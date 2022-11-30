using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using NETCore.Encrypt.Extensions;
using Shared.Entities;
using Shared.Helpers;
using Shared.Models;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace Shared.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        #region Constractor
         private readonly DatabaseContext _context;
        private readonly IConfiguration _configuration;
        private readonly IHasher _hasher;
        public AccountController(DatabaseContext context, IConfiguration configuration, IHasher hasher)
        {
            _context = context;
            _configuration = configuration;
            _hasher = hasher;
        }
        #endregion
        #region Login
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                string hashedPassword = _hasher.DoMD5HashedString(model.Password);
                User user = _context.Users.SingleOrDefault(x => x.Username.ToLower() == model.Username.ToLower() && x.Password == hashedPassword);
                if (user != null)
                {
                    if (user.Locked)
                    {
                        ModelState.AddModelError(nameof(model.Username), "User is locked.");
                        return View(model);
                    }
                    //Cookie Authentication
                    List<Claim> claims = new List<Claim>();
                    claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
                    claims.Add(new Claim(ClaimTypes.Name, user.FullName ?? String.Empty));
                    claims.Add(new Claim(ClaimTypes.Role, user.Role));
                    claims.Add(new Claim("Username", user.Username));

                    ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    ClaimsPrincipal principal = new ClaimsPrincipal(identity);

                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Username or password is incorrect.");
                }

            }
            return View(model);
        }
        #endregion
        #region Register
         [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous] 
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (_context.Users.Any(x => x.Username.ToLower() == model.Username.ToLower()))
                {
                    ModelState.AddModelError(nameof(model.Username), "Username is already exists.");
                    View(model);
                }
                string hashedPassword = _hasher.DoMD5HashedString(model.Password);
                User user = new()
                {
                    Username = model.Username,
                    Password = hashedPassword
                };
                _context.Users.Add(user);
                int affectedRowCount=_context.SaveChanges();
                if(affectedRowCount==0)
                {
                    ModelState.AddModelError("", "User can not be added.");
                }
                else
                {
                    return RedirectToAction(nameof(Login));//Action adı değişirse bana hata ver!
                }
            }
            return View(model);
        }
        #endregion
        #region Profile
         public IActionResult Profile()
        {
            ProfileInfoLoader();
            return View();
        }

        private void ProfileInfoLoader()
        {
            Guid userid = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));
            User user = _context.Users.SingleOrDefault(x => x.Id == userid);
            ViewData["FullName"] = user.FullName;
            ViewData["ProfileImage"] = user.ProfileImageFileName;
        }

        [HttpPost]
        public IActionResult ProfileChangeFullName([Required][StringLength(50)] string? fullName)
        {
            if (ModelState.IsValid)
            {
                Guid userid = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));
                User user = _context.Users.SingleOrDefault(x => x.Id == userid);

                user.FullName=fullName;
                _context.SaveChanges();

                return RedirectToAction(nameof(Profile));
            }
            ProfileInfoLoader();
            return View("Profile");
        }
        [HttpPost]
        public IActionResult ProfileChangeImage([Required]IFormFile file)
        {
            if (ModelState.IsValid)
            {
                Guid userid = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));
                User user = _context.Users.SingleOrDefault(x => x.Id == userid);

                //p_guid.jpg
                string fileName = $"p_{userid}.jpg";
                //string fileName = $"p_{userid}.{file.ContentType.Split('/')[1]}";
                Stream stream = new FileStream($"wwwroot/uploads/{fileName}",FileMode.OpenOrCreate);
                file.CopyTo(stream);

                stream.Close();
                stream.Dispose();

                user.ProfileImageFileName = fileName;
                _context.SaveChanges();

                return RedirectToAction(nameof(Profile));
            }
            ProfileInfoLoader();
            return View("Profile");
        }
        [HttpPost]
        public IActionResult ProfileChangePassword([Required][MinLength(6)][MaxLength(16)] string? password)
        {
            if (ModelState.IsValid)
            {
                Guid userid = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));
                User user = _context.Users.SingleOrDefault(x => x.Id == userid);
                string hashedPassword = _hasher.DoMD5HashedString(password);
                user.Password = hashedPassword;
                _context.SaveChanges();

                ViewData["result"] = "PasswordChanged";
            }
            ProfileInfoLoader();
            return View("Profile");
        }
        #endregion
        #region LogOut
         public IActionResult Logout()
        {
           HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Login));
        }
        #endregion
    }
}

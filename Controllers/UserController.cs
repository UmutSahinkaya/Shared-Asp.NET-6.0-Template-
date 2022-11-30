using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Shared.Entities;
using Shared.Helpers;
using Shared.Models;

namespace Shared.Controllers
{
    public class UserController : Controller
    {
        #region Constractor
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;
        private readonly IHasher _hasher;
        public UserController(DatabaseContext context, IMapper mapper, IHasher hasher)
        {
            _context = context;
            _mapper = mapper;
            _hasher = hasher;
        }
        #endregion
        #region Actions of Get
        public IActionResult Index()
        {
            List<UserViewModel> users =
                _context.Users.ToList().
                    Select(x=>_mapper.Map<UserViewModel>(x)).ToList();
            return View(users);
        }
        public IActionResult Create()
        {
            return View();
        }
        public IActionResult Edit(Guid id)
        {
            User user = _context.Users.Find(id);
            EditUserModel model = _mapper.Map<EditUserModel>(user);
            return View(model);
        }
        public IActionResult Delete(Guid id)
        {
            User user = _context.Users.Find(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
        #endregion
        #region Actions of Post
        [HttpPost]
        public IActionResult Create(CreateUserModel model)
        {
            if(ModelState.IsValid)
            {
                if(_context.Users.Any(x=>x.Username.ToLower() == model.Username.ToLower()))
                {
                    ModelState.AddModelError(nameof(model.Username), "Username is already exists!");
                    return View(model);
                }
                User user= _mapper.Map<User>(model);
                user.Password=_hasher.DoMD5HashedString(model.Password);
                _context.Users.Add(user);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
        [HttpPost]
        public IActionResult Edit(Guid id,EditUserModel model)
        {
            if (ModelState.IsValid)
            {
                if (_context.Users.Any(x => x.Username.ToLower() == model.Username.ToLower() && x.Id!= id))
                {
                    ModelState.AddModelError(nameof(model.Username), "Username is already exists!");
                    return View(model);
                }
                User user = _context.Users.Find(id);
                _mapper.Map(model, user);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
        #endregion
    }
}

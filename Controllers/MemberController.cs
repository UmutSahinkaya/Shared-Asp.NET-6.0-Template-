using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Shared.Entities;
using Shared.Helpers;
using Shared.Models;

namespace Shared.Controllers
{
    public class MemberController : Controller
    {
        #region Constractor
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;
        private readonly IHasher _hasher;
        public MemberController(IMapper mapper, DatabaseContext context = null, IHasher hasher = null)
        {
            _mapper = mapper;
            _context = context;
            _hasher = hasher;
        }
        #endregion
        #region Actions of Get
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult MemberListPartial()
        {
            List<UserViewModel> users = _context.Users.ToList()
                .Select(x => _mapper.Map<UserViewModel>(x)).ToList();
            return PartialView("_MemberListPartial",users);
        }
        public IActionResult EditUserPartial(Guid id)
        {
            User user = _context.Users.Find(id);
            EditUserModel model = _mapper.Map<EditUserModel>(user);

            return PartialView("_EditUserPartial", model);
        }
        public IActionResult DeleteUser(Guid id)
        {
            User user = _context.Users.Find(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
            return MemberListPartial();
        }
        public IActionResult AddNewUserPartial()
        {
            return PartialView("_AddNewUserPartial", new CreateUserModel());
        }
        #endregion
        #region Actions of Post
        [HttpPost]
        public IActionResult EditUser(Guid id, EditUserModel model)
        {
            if (ModelState.IsValid)
            {
                if (_context.Users.Any(x => x.Username.ToLower() == model.Username.ToLower() && x.Id != id))
                {
                    ModelState.AddModelError(nameof(model.Username), "Username is already exists!");
                    return PartialView("_EditUserPartial", model);
                }
                User user = _context.Users.Find(id);
                _mapper.Map(model, user);
                _context.SaveChanges();
                return PartialView("_EditUserPartial", new EditUserModel { Done = "User updated." });
            }
            return PartialView("_EditUserPartial", model);
        }
        
        [HttpPost]
        public IActionResult AddNewUser(CreateUserModel model)
        {
            if (ModelState.IsValid)
            {
                if (_context.Users.Any(x => x.Username.ToLower() == model.Username.ToLower()))
                {
                    ModelState.AddModelError(nameof(model.Username), "Username is already exists!");
                    return PartialView("_AddNewUserPartial", model);
                }
                User user = _mapper.Map<User>(model);
                user.Password=_hasher.DoMD5HashedString(model.Password);
                _context.Users.Add(user);
                _context.SaveChanges();
                return PartialView("_AddNewUserPartial", new CreateUserModel { Done="User added."});
            }
            return PartialView("_AddNewUserPartial", model);
        }
        #endregion
    }
}
//TODO:asd

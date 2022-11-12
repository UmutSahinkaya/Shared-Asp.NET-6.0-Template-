using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Shared.Entities;
using Shared.Models;

namespace Shared.Controllers
{
    public class UserController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;

        public UserController(DatabaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

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
        public IActionResult Delete(Guid id)
        {   User user=_context.Users.Find(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}

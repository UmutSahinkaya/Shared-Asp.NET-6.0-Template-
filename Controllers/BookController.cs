using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Shared.Entities;
using Shared.Models;

namespace Shared.Controllers
{
    public class BookController : Controller
    {
        #region Constractor
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;

        public BookController(IMapper mapper, DatabaseContext context)
        {
            _mapper = mapper;
            _context = context;
        }
        #endregion
        #region Actions of Get
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult BookListPartial()
        {
            List<BookViewModel> books= _context.Books.ToList().Select(x=> _mapper.Map<BookViewModel>(x)).ToList();
            return PartialView("_BookListPartial",books);
        }
        public IActionResult EditBook(Guid id)
        {
            Book book = _context.Books.Find(id);
            EditBookModel model = _mapper.Map<EditBookModel>(book);

            return PartialView("_EditBookPartial", model);
        }
        public IActionResult DeleteBook(Guid id)
        {
            Book book = _context.Books.Find(id);
            if (book != null)
            {
                _context.Books.Remove(book);
                _context.SaveChanges();
            }
            return BookListPartial();
        }
        public IActionResult AddNewBookPartial()
        {
            return PartialView("_AddNewBookPartial", new CreateBookModel());
        }
        #endregion
        #region Actions of Post
        [HttpPost]
        public IActionResult EditBook(Guid id, EditBookModel model)
        {
            if (ModelState.IsValid)
            {//TODO:Add writername
                if (_context.Books.Any(x => x.BookName.ToLower() == model.BookName.ToLower() && x.Id != id))
                {
                    model.Stock += 1;

                    return PartialView("_EditBookPartial", model);
                }
                Book book = _context.Books.Find(id);
                _mapper.Map(model, book);
                _context.SaveChanges();
                return PartialView("_EditBookPartial", new EditBookModel { Done = "Book is updated." });
            }
            return PartialView("_EditBookPartial", model);
        }

        [HttpPost]
        public IActionResult AddNewBook(CreateBookModel model)
        {
            if (ModelState.IsValid)
            {
                //TODO:Add writername
                if (_context.Books.Any(x => x.BookName.ToLower() == model.BookName.ToLower()))
                {
                    model.Stock += 1;
                    return PartialView("_AddNewBookPartial", model);
                }
                Book book = _mapper.Map<Book>(model);
                
                _context.Books.Add(book);
                _context.SaveChanges();
                return PartialView("_AddNewBookPartial", new CreateBookModel { Done = "Book added." });
            }
            return PartialView("_AddNewBookPartial", model);
        }
        #endregion
    }
}

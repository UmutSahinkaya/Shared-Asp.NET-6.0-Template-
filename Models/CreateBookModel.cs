using System.ComponentModel.DataAnnotations;

namespace Shared.Models
{
    public class CreateBookModel
    {
        [StringLength(100, ErrorMessage = "Book name can be max 100 characters.")]
        [Required(ErrorMessage = "Book name is required!")]
        public string BookName { get; set; }
        [StringLength(100, ErrorMessage = "Book title can be max 100 characters.")]
        [Required(ErrorMessage = "Book title is required!")]
        public string BookTitle { get; set; }
        [StringLength(50, ErrorMessage = "Writer name can be max 50 characters.")]
        public string? WriterName { get; set; }
        public DateTime DateOfPrint { get; set; }
        public bool IsStock { get; set; }
        public int Stock { get; set; }
        public string? Done { get; set; }
    }
}

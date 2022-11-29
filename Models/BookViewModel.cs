namespace Shared.Models
{
    public class BookViewModel
    {
        public Guid Id { get; set; }
        public string BookName { get; set; }
        public string BookTitle { get; set; }
        public string? WriterName { get; set; }
        public DateTime GivenDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public DateTime DateOfPrint { get; set; }
        public bool IsStock { get; set; }
        public int Stock { get; set; }
    }
}

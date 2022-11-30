using Castle.Components.DictionaryAdapter;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KeyAttribute = System.ComponentModel.DataAnnotations.KeyAttribute;

namespace Shared.Entities
{
    [Table("Books")]
    public class Book
    {
        [Key]
        public Guid Id { get; set; }
        [StringLength(100)]
        [Required]
        public string BookName { get; set; }
        [StringLength(100)]
        [Required]
        public string BookTitle { get; set; }
        [StringLength(50)]
        public string? WriterName { get; set; }
        public DateTime GivenDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public DateTime DateOfPrint { get; set; }
        public bool IsStock { get; set; }
        public int Stock { get; set; }
    }
}

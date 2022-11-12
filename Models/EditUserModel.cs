using System.ComponentModel.DataAnnotations;

namespace Shared.Models
{
    public class EditUserModel
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Username is required.")]
        [StringLength(30, ErrorMessage = "Username can be max 30 characters.")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Fullname is required.")]
        [StringLength(50, ErrorMessage = "Fullname can be max 50 characters.")]
        public string Fullname { get; set; }
        public bool Locked { get; set; }
        [Required]
        [StringLength(50)]
        public string Role { get; set; } = "user";
        public string? Done { get; set; }
    }
}

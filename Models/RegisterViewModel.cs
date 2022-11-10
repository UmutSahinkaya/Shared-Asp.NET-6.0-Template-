using System.ComponentModel.DataAnnotations;

namespace Shared.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Username is required.")]
        [StringLength(30, ErrorMessage = "Username can be max 30 characters.")]
        public string Username { get; set; }

        //[DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required.")]
        [MaxLength(16, ErrorMessage = "Password can be max 16 characters.")]
        [MinLength(6, ErrorMessage = "Password can be min 6 characters.")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Re-Password is required.")]
        [MaxLength(16, ErrorMessage = "Re-Password can be max 16 characters.")]
        [MinLength(6, ErrorMessage = "Re-Password can be min 6 characters.")]
        [Compare(nameof(Password))]
        public string RePassword { get; set; }
    }
}

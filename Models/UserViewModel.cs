using System.ComponentModel.DataAnnotations;

namespace Shared.Models
{
    public class UserViewModel
    {
        public Guid Id { get; set; }
        public string? FullName { get; set; }
        public string Username { get; set; }
        public bool Locked { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string? ProfileImageFileName { get; set; } = "no-image.jpg";
        public string Role { get; set; } = "user";
    }
}

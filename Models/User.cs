using System.ComponentModel.DataAnnotations;

namespace BunnyMonitor.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string UserName { get; set; }

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(1)]
        public string Password { get; set; }

        public string Token { get; set; }
    }
}

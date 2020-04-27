using AntiPlagiatusServer.Models.DTO;
using System.ComponentModel.DataAnnotations;

namespace AntiPlagiatusServer.Models
{
    public class UserViewModel
    {
        public UserViewModel()
        {
        }
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        [EmailAddress]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
        public string DefaultTheme { get; set; }
    }
}

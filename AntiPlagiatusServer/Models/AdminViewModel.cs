using System.ComponentModel.DataAnnotations;

namespace AntiPlagiatusServer.Data
{
    public class AdminViewModel
    {
        [Key]
        [Required]
        [EmailAddress]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }
    }
}

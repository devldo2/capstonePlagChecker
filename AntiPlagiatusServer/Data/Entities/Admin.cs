using System.ComponentModel.DataAnnotations;

namespace AntiPlagiatusServer.Data.Entities
{
    public class Admin
    {
        [Key]
        [Required]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
    }
}

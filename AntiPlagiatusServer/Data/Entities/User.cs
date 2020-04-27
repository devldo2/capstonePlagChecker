using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace AntiPlagiatusServer.Data.Entities
{
    public class User : EntityBase
    {
        public User()
        {
            this.OperationReports = new Collection<OperationReport>();
        }
        [Required]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Token { get; set; }
        public string DefaultTheme { get; set; }
        public virtual ICollection<OperationReport> OperationReports { get; set; }
    }
}

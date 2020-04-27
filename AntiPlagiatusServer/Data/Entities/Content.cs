using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AntiPlagiatusServer.Data.Entities
{
    public class Content: EntityBase
    {
        [Required]
        public string Text { get; set; }
        [Required]
        public string Origin { get; set; }
        public virtual ICollection<OperationReport> OperationReports { get; set; }
    }
}

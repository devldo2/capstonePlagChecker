using System.ComponentModel.DataAnnotations;

namespace AntiPlagiatusServer.Data.Entities
{
    public class IgnoreRule: EntityBase
    {
        [Required]
        public string Url { get; set; }
        public string Type { get; set; }
        public int OperationReportId { get; set; }
        public virtual OperationReport OperationReport { get; set; }
    }
}

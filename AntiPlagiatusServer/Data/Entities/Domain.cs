using System.Collections.Generic;

namespace AntiPlagiatusServer.Data.Entities
{
    public class Domain: EntityBase
    {
        public string Uri { get; set; }
        public int Rewrite { get; set; }
        public int Equality { get; set; }
        public virtual ICollection<LayerByDomain> Layers { get; set; }
        public int OperationReportId { get; set; }
        public virtual OperationReport OperationReport { get; set; }
    }
}

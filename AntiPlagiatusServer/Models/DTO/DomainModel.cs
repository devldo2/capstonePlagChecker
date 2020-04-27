using System.Collections.Generic;

namespace AntiPlagiatusServer.Models.DTO
{
    public class DomainModel
    {
        public string Uri { get; set; }
        public int Rewrite { get; set; }
        public int Equality { get; set; }
        public List<LayerByDomainModel> Layers { get; set; }
    }
}

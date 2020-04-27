using System.Collections.Generic;

namespace AntiPlagiatus.Models.UI
{
    public class Domain
    {
        public string Uri { get; set; }
        public int Rewrite { get; set; }
        public int Equality { get; set; }
        public List<LayerByDomain> Layers { get; set; }
    }
}

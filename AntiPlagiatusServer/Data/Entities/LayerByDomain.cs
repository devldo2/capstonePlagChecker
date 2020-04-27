namespace AntiPlagiatusServer.Data.Entities
{
    public class LayerByDomain: EntityBase
    {
        public int Rewrite { get; set; }
        public string Words { get; set; }
        public string Uri { get; set; }
        public int Equality { get; set; }
        public int DomainId { get; set; }
        public virtual Domain Domain { get; set; }
    }
}

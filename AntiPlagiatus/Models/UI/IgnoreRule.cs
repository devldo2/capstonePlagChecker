namespace AntiPlagiatus.Models.UI
{
    public class IgnoreRule
    {
        public string Url { get; set; }
        public string FullPath => this.Type == IgnoreType.Domain ? $"http://{this.Url}" : this.Url;
        public IgnoreType Type { get; set; }
    }
}

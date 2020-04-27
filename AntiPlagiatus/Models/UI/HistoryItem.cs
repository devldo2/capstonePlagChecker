using System;
using System.Collections.Generic;

namespace AntiPlagiatus.Models.UI
{
    public class HistoryItem
    {
        public string ReportId { get; set; }
        public Content Content { get; set; }
        public List<IgnoreRule> IgnoreRules { get; set; }
        public Status Status { get; set; }
        public int Equality { get; set; }
        public int Rewrite { get; set; }
        public int CharactersNumber { get; set; }
        public int WordCount { get; set; }
        public List<Domain> Domains { get; set; }
        public string ErrorMessage { get; set; }
        public DateTime Date { get; set; }
        public string UserToken { get; set; }
    }
}

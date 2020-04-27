using System;
using System.Collections.Generic;

namespace AntiPlagiatus.Models.UI
{
    public class ReportItem
    {
        public Status Status { get; set; }
        public int Equality { get; set; } = 100;
        public int Rewrite { get; set; } = 100;
        public int CharactersNumber { get; set; }
        public int WordCount { get; set; }
        public List<Domain> Domains { get; set; } = new List<Domain>();
        public string ErrorMessage { get; set; }
        public DateTime Date { get; set; }
    }
}

using System.Collections.Generic;

namespace AntiPlagiatusServer.Models.DTO
{
    public class OperationReportModel
    {
        public string ReportId { get; set; }
        public string Date { get; set; }
        public string Status { get; set; }
        public ContentModel Content { get; set; }
        public List<IgnoreRuleModel> IgnoreRules { get; set; }
        public int Equality { get; set; }
        public int Rewrite { get; set; }
        public int CharactersNumber { get; set; }
        public int WordCount { get; set; }
        public List<DomainModel> Domains { get; set; }
        public string ErrorMessage { get; set; }
        public string UserToken { get; set; }
    }
}

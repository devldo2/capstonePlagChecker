using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace AntiPlagiatusServer.Data.Entities
{
    public class OperationReport : EntityBase
    {
        public OperationReport()
        {
            this.IgnoreRules = new Collection<IgnoreRule>();
        }
        [Required]
        public string ReportId { get; set; }
        [Required]
        public string Date { get; set; }
        [Required]
        public string Status { get; set; }
        [Required]
        public int ContentId { get; set; }
        [Required]
        public virtual Content Content { get; set; }
        public virtual ICollection<IgnoreRule> IgnoreRules { get; set; }
        public int Equality { get; set; }
        public int Rewrite { get; set; }
        public int CharactersNumber { get; set; }
        public int WordCount { get; set; }
        public virtual ICollection<Domain> Domains { get; set; }
        public string ErrorMessage { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public virtual User User { get; set; }
    }
}

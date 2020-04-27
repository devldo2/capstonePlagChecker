using AntiPlagiatus.Helpers.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace AntiPlagiatus.Models.API
{
    [DataContract]
    public class APIReport
    {
        public int Id { get; set; }
        public int CheckId { get; set; }
        [DataMember]
        public string ReportId { get; set; }
        [DataMember]
        public APIContent Content { get; set; }
        [DataMember]
        public List<APIIgnoreRule> IgnoreRules { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public int Equality { get; set; }
        [DataMember]
        public int Rewrite { get; set; }
        [DataMember]
        public int CharactersNumber { get; set; }
        [DataMember]
        public int WordCount { get; set; }
        [DataMember]
        public List<APIDomain> Domains { get; set; }
        [DataMember]
        public string ErrorMessage { get; set; }
        [DataMember]
        public string Date { get; private set; }
        [DataMember]
        public string UserToken { get; set; }
    }
    [DataContract]
    public class APIContent
    {
        [DataMember]
        public string Text { get; set; }
        [DataMember]
        public string Origin { get; set; }
    }
    [DataContract]
    public class APIIgnoreRule
    {
        public int Id { get; set; }
        [DataMember]
        public string Url { get; set; }
        [DataMember]
        public string Type { get; set; }
    }
    [DataContract]
    public class APIDomain
    {
        [DataMember]
        public string Uri { get; set; }
        [DataMember]
        public int Rewrite { get; set; }
        [DataMember]
        public int Equality { get; set; }
        [DataMember]
        public List<APILayerByDomain> Layers { get; set; }
    }
    [DataContract]
    public class APILayerByDomain
    {
        [DataMember]
        public int Rewrite { get; set; }
        [DataMember]
        public string StringWords { get; set; }
        [DataMember]
        public string Uri { get; set; }
        [DataMember]
        public int Equality { get; set; }
    }
}

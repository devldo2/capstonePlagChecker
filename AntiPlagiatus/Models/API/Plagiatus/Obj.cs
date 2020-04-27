using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace AntiPlagiatus.Models
{
    [DataContract]
    public abstract class EntityBaseApi
    {
        [DataMember]
        public string id { get; set; }
        [DataMember]
        public string jsonrpc { get; set; }
    }

    //request object
    [DataContract]
    public class RequestObj : EntityBaseApi
    {
        [DataMember]
        public string method { get; set; }
        [DataMember]
        public Param @params { get; set; }
    }

    [DataContract]
    public class Param
    {
        [DataMember]
        public string token { get; set; }
        [DataMember]
        public string text { get; set; }
        //[DataMember]
        //public string title { get; set; } = Guid.NewGuid().ToString();
        [DataMember]
        public Option options { get; set; }

        [DataMember]
        public string report_json { get; set; }
        [DataMember]
        public string key { get; set; }
    }

    [DataContract]
    public class Option
    {
        [DataMember]
        public List<string> ignore_rules { get; set; }
    }

    //check response
    [DataContract]
    public class CheckResponse : EntityBaseApi
    {
        [DataMember]
        public CheckResponseResult result { get; set; }
    }

    [DataContract]
    public class CheckResponseResult
    {
        [DataMember]
        public string key { get; set; }

        [DataMember]
        public string error_msg { get; set; }

        [DataMember]
        public int error { get; set; }
    }

    //report response
    [DataContract]
    public class ReportResponse : EntityBaseApi
    {
        [DataMember]
        public ReportResponseResult result { get; set; }
    }

    [DataContract]
    public class ReportResponseResult
    {
        [DataMember]
        public Report report { get; set; }

        [DataMember]
        public string hash_url { get; set; }

        [DataMember]
        public object is_public { get; set; }

        [DataMember]
        public string status { get; set; }

        [DataMember]
        public object is_fixed { get; set; }

        [DataMember]
        public string error_msg { get; set; }

        [DataMember]
        public int error { get; set; }
    }

    [DataContract]
    public class Layer
    {
        [DataMember]
        public int rewrite { get; set; }

        [DataMember]
        public List<int> words { get; set; }

        [DataMember]
        public string uri { get; set; }

        [DataMember]
        public int equality { get; set; }

        [DataMember]
        public List<object> shingles { get; set; }
    }

    [DataContract]
    public class LayersByDomain
    {
        [DataMember]
        public string domain { get; set; }

        [DataMember]
        public int rewrite { get; set; }

        [DataMember]
        public int equality { get; set; }

        [DataMember]
        public List<Layer> layers { get; set; }
    }

    [DataContract]
    public class Layer2
    {
        [DataMember]
        public List<int> words { get; set; }

        [DataMember]
        public string uri { get; set; }

        [DataMember]
        public int equality { get; set; }

        [DataMember]
        public int rewrite { get; set; }

        [DataMember]
        public List<object> shingles { get; set; }
    }

    [DataContract]
    public class Report
    {
        [DataMember]
        public List<LayersByDomain> layers_by_domain { get; set; }

        [DataMember]
        public int checked_pages { get; set; }

        [DataMember]
        public int equality { get; set; }

        [DataMember]
        public int rewrite { get; set; }

        [DataMember]
        public List<string> text_fragments { get; set; }

        [DataMember]
        public int progress { get; set; }

        [DataMember]
        public List<int> sym_bins { get; set; }

        [DataMember]
        public List<int> equality_per_bin { get; set; }

        [DataMember]
        public int word_count { get; set; }

        [DataMember]
        public string lang { get; set; }

        [DataMember]
        public int error_phrases { get; set; }

        [DataMember]
        public int checked_phrases { get; set; }

        [DataMember]
        public string id { get; set; }

        [DataMember]
        public int found_pages { get; set; }

        [DataMember]
        public int error_pages { get; set; }

        [DataMember]
        public List<int> equal_shingles { get; set; }

        [DataMember]
        public List<int> equal_shingle_words { get; set; }

        [DataMember]
        public int layers_cnt { get; set; }

        [DataMember]
        public int captchas { get; set; }

        [DataMember]
        public List<int> word_bins { get; set; }

        [DataMember]
        public int domains_cnt { get; set; }

        [DataMember]
        public List<Layer2> layers { get; set; }

        [DataMember]
        public List<object> bad_words { get; set; }

        [DataMember]
        public List<int> equal_words { get; set; }

        [DataMember]
        public object urls_stats { get; set; }

        [DataMember]
        public List<int> rewrite_per_bin { get; set; }

        [DataMember]
        public int len { get; set; }
    }

    
}

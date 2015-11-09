using System.Collections.Generic;

namespace iFixit.Domain.Models.REST.V1_0.Search.Device
{
    public class Info
    {
        public string introduced_date { get; set; }
        public string discontinued_date { get; set; }
        public string manufacturer { get; set; }
        public string __invalid_name__ { get; set; }
    }

    public class Result
    {
        public int expire { get; set; }
        public string @namespace { get; set; }
        public string title { get; set; }
        public string display_title { get; set; }
        public string summary { get; set; }
        public int id { get; set; }
        public int imageid { get; set; }
        public string guid { get; set; }
        public object ads_image_url { get; set; }
        public string info_names { get; set; }
        public string info_values { get; set; }
        public List<string> flags { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string url { get; set; }
        public string mini { get; set; }
        public string thumbnail { get; set; }
        public string standard { get; set; }
        public string medium { get; set; }
        public string @class { get; set; }
        public string wiki_title { get; set; }
        public int wikiid { get; set; }
        public string text { get; set; }
        public Info info { get; set; }
    }

    public class RootObject
    {
        public string search { get; set; }
        public List<Result> results { get; set; }
    }
}

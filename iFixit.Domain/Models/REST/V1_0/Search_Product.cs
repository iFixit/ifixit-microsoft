using System.Collections.Generic;

namespace iFixit.Domain.Models.REST.V1_0.Search.Product
{
    public class Result
    {
        public int expire { get; set; }
        public string title { get; set; }
        public string text { get; set; }
        public string id { get; set; }
        public string image_url { get; set; }
        public string target_url { get; set; }
        public double price { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string url { get; set; }
        public string mini { get; set; }
        public string thumbnail { get; set; }
        public string @class { get; set; }
        public string productcode { get; set; }
        public int optionid { get; set; }
        public string itemcode { get; set; }
    }

    public class RootObject
    {
        public string search { get; set; }
        public List<Result> results { get; set; }
    }
}

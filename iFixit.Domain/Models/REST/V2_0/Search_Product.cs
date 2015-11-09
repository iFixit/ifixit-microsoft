using System.Collections.Generic;

namespace iFixit.Domain.Models.REST.V2_0.Search.Product
{
    public class Result
    {
        public string dataType { get; set; }
        public string title { get; set; }
        public string text { get; set; }
        public string url { get; set; }
        public double price { get; set; }
        public string productCode { get; set; }
        public Image image { get; set; }
    }

    public class Image
    {
        public string mini { get; set; }
        public string thumbnail { get; set; }
        public string original { get; set; }
    }

    public class RootObject
    {
        public int totalResults { get; set; }
        public bool moreResults { get; set; }
        public string search { get; set; }
        public List<Result> results { get; set; }
    }
}

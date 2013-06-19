using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iFixit.Domain.Models.REST.V1_1.Search.Device
{
    public class Image
    {
        public int id { get; set; }
        public string mini { get; set; }
        public string thumbnail { get; set; }
        public string standard { get; set; }
        public string medium { get; set; }
        public string large { get; set; }
        public string original { get; set; }
    }

    public class Result
    {
        public string dataType { get; set; }
        public string title { get; set; }
        public string display_title { get; set; }
        public string @namespace { get; set; }
        public string summary { get; set; }
        public string url { get; set; }
        public string text { get; set; }
        public Image image { get; set; }
    }

    public class RootObject
    {
        public string totalResults { get; set; }
        public bool moreResults { get; set; }
        public string search { get; set; }
        public List<Result> results { get; set; }
    }
}

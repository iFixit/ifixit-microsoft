using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iFixit.Domain.Models.REST.V1_0.Search.Guide
{
    public class Result
    {
        public int expire { get; set; }
        public string type { get; set; }
        public string device { get; set; }
        public string thing { get; set; }
        public string text { get; set; }
        public int id { get; set; }
        public int imageid { get; set; }
        public string title { get; set; }
        public int guideid { get; set; }
        public int userid { get; set; }
        public string username { get; set; }
        public int @public { get; set; }
        public int? teamid { get; set; }
        public string flags { get; set; }
        public string summary { get; set; }
        public string guid { get; set; }
        public string shortTitle { get; set; }
        public string simpleTitle { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public int modified_date { get; set; }
        public int? prereq_modified_date { get; set; }
        public string mini { get; set; }
        public string thumbnail { get; set; }
        public string standard { get; set; }
        public string medium { get; set; }
        public string large { get; set; }
        public string author { get; set; }
        public string guideType { get; set; }
        public string @class { get; set; }
    }

    public class RootObject
    {
        public string search { get; set; }
        public List<Result> results { get; set; }
    }
}

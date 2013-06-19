using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iFixit.Domain.Models.REST.V1_1.Guides
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

    public class RootObject
    {
        public int guideid { get; set; }
        public bool revisionid { get; set; }
        public int modified_date { get; set; }
        public int? prereq_modified_date { get; set; }
        public string type { get; set; }
        public string topic { get; set; }
        public string subject { get; set; }
        public string title { get; set; }
        public bool @public { get; set; }
        public int userid { get; set; }
        public string username { get; set; }
        public Image image { get; set; }
    }
}

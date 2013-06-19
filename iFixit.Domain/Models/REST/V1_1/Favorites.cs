using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iFixit.Domain.Models.REST.V1_1.Favorites
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

    public class Guide
    {
        public string dataType { get; set; }
        public int guideid { get; set; }
        public int revisionid { get; set; }
        public int modified_date { get; set; }
        public object prereq_modified_date { get; set; }
        public string url { get; set; }
        public string type { get; set; }
        public string category { get; set; }
        public string subject { get; set; }
        public string title { get; set; }
        public bool @public { get; set; }
        public int userid { get; set; }
        public string username { get; set; }
        public Image image { get; set; }
    }

    public class RootObject
    {
        public int date { get; set; }
        public Guide guide { get; set; }
    }
}

using System.Collections.Generic;

namespace iFixit.Domain.Models.REST.V2_0
{
    public class Collections : List<Collection>
    {


    }
    public class Collection
    {
        public int collectionid { get; set; }
        public string title { get; set; }
        public int date { get; set; }
        public CollectionImage image { get; set; }
        public CollectionGuide[] guides { get; set; }
    }

   

    public class CollectionGuide
    {
        public string dataType { get; set; }
        public int guideid { get; set; }
        public string locale { get; set; }
        public int revisionid { get; set; }
        public float modified_date { get; set; }
        public float prereq_modified_date { get; set; }
        public string url { get; set; }
        public string type { get; set; }
        public string category { get; set; }
        public string subject { get; set; }
        public string title { get; set; }
        public bool _public { get; set; }
        public int userid { get; set; }
        public string username { get; set; }
        public string[] flags { get; set; }
        public CollectionImage image { get; set; }
    }

   

    public class CollectionImage
    {
        public int id { get; set; }
        public string guid { get; set; }
        public string mini { get; set; }
        public string thumbnail { get; set; }
        public string standard { get; set; }
        public string medium { get; set; }
        public string large { get; set; }
        public string huge { get; set; }
        public string original { get; set; }
    }

}

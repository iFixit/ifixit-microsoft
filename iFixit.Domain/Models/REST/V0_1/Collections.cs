using System;
using System.Net;
using System.Windows;

using System.Collections.Generic;

namespace iFixit.Domain.Models.REST.V0_1
{
    public class Collections : List<collection>
    {

    }

    public class collection
    {
        public int collectionid { get; set; }
        public string title { get; set; }
        public double date { get; set; }
        public List<int> guideids { get; set; }
        public CollectionImage image { get; set; }
    }

    public class CollectionImage
    {
        public string mini { get; set; }
        public string thumbnail { get; set; }
        public string standard { get; set; }
        public string medium { get; set; }
        public string large { get; set; }
    }
}

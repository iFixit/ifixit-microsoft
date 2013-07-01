using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iFixit.Domain.Models.REST
{
    public class RssFeed
    {
        public string Version { get; set; }
        public RssChannel channel { get; set; }
    }

    public class RssChannel
    {
        public string title { get; set; }
        public string description { get; set; }
        public string pubDate { get; set; }
        public RssItems item { get; set; }
    }

    public class RssItems : List<item> { }

    public class item
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Strapline { get; set; }
        public string Lead { get; set; }
        public string Pretitle { get; set; }
        public string PubDate { get; set; }
        public string imagename { get; set; }
        public string imagecredits { get; set; }
        public string imagecaption { get; set; }
        public content content { get; set; }
        public thumbnail thumbnail { get; set; }
        public string category { get; set; }
        public string encoded { get; set; }
        public string videourl { get; set; }
        public string Link { get; set; }
        public string Author { get; set; }

        public string id { get; set; }
    }

    public class thumbnail : image { }
    public class content : image { }

    public class image
    {
        public string url { get; set; }
        public string type { get; set; }
        public string width { get; set; }
        public string height { get; set; }
    }
}

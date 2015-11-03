using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iFixit.Domain.Models.REST.V1_0.Guide
{
    public class Author
    {
        public string text { get; set; }
        public int userid { get; set; }
    }

    public class Document
    {
        public string text { get; set; }
        public string url { get; set; }
        public int documentid { get; set; }
    }

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

    public class Part
    {
        public string text { get; set; }
        public string notes { get; set; }
        public string type { get; set; }
        public string quantity { get; set; }
        public string url { get; set; }
        public string thumbnail { get; set; }
    }

    public class Prereq
    {
        public string text { get; set; }
        public int guideid { get; set; }
        public string locale { get; set; }
    }

    public class Line
    {
        public string text_raw { get; set; }
        public string bullet { get; set; }
        public int level { get; set; }
        public int? lineid { get; set; }
        public string text_rendered { get; set; }
    }

    public class Datum
    {
        public int id { get; set; }
        public string mini { get; set; }
        public string thumbnail { get; set; }
        public string standard { get; set; }
        public string medium { get; set; }
        public string large { get; set; }
        public string original { get; set; }
    }

    public class Media
    {
        public string type { get; set; }
        public List<Datum> data { get; set; }
    }

    public class Step
    {
        public string title { get; set; }
        public List<Line> lines { get; set; }
        public Media media { get; set; }
        public int guideid { get; set; }
        public int stepid { get; set; }
        public int orderby { get; set; }
        public int revisionid { get; set; }
    }

    public class Tool
    {
        public string type { get; set; }
        public string quantity { get; set; }
        public string text { get; set; }
        public string notes { get; set; }
        public string url { get; set; }
        public string thumbnail { get; set; }
    }

    public class Guide
    {
        public Author author { get; set; }
        public List<string> categories { get; set; }
        public string conclusion_raw { get; set; }
        public string conclusion_rendered { get; set; }
        public string difficulty { get; set; }
        public List<Document> documents { get; set; }
        public List<object> flags { get; set; }
        public int guideid { get; set; }
        public Image image { get; set; }
        public string introduction_raw { get; set; }
        public string introduction_rendered { get; set; }
        public string locale { get; set; }
        public List<Part> parts { get; set; }
        public List<Prereq> prereqs { get; set; }
        public List<Step> steps { get; set; }
        public string subject { get; set; }
        public object summary { get; set; }
        public string time_required { get; set; }
        public string title { get; set; }
        public List<Tool> tools { get; set; }
        public string type { get; set; }
        public int revisionid { get; set; }
        public bool @public { get; set; }
    }

    public class RootObject
    {
        public string topic { get; set; }
        public string url { get; set; }
        public Guide guide { get; set; }
        public int guideid { get; set; }
        public bool canEdit { get; set; }
    }
}

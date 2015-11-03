using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iFixit.Domain.Models.REST.V2_0.Guide
{
   
      public class Part
    {
        public string text { get; set; }
        public string notes { get; set; }
        public string type { get; set; }
        public string quantity { get; set; }
        public string url { get; set; }
        public string thumbnail { get; set; }
    }

  
    public class Prerequisite
    {
        public string dataType { get; set; }
        public int guideid { get; set; }
        public string locale { get; set; }
        public int revisionid { get; set; }
        public double modified_date { get; set; }
        public double prereq_modified_date { get; set; }
        public string url { get; set; }
        public string type { get; set; }
        public string category { get; set; }
        public string subject { get; set; }
        public string title { get; set; }
        public bool @public { get; set; }
        public int userid { get; set; }
        public string username { get; set; }
        public List<string> flags { get; set; }
        public Image image { get; set; }
    }

    public class Line
    {
        public string text_raw { get; set; }
        public string bullet { get; set; }
        public int level { get; set; }
        public int? lineid { get; set; }
        public string text_rendered { get; set; }
    }


    public class Image
    {
        public int id { get; set; }
        public string mini { get; set; }
        public string thumbnail { get; set; }
        public string standard { get; set; }
        public string medium { get; set; }
        public string large { get; set; }
        public string huge { get; set; }
        public string original { get; set; }
    }

    public class ImageVideo
    {
        public Image image { get; set; }
    }

    public class Video
    {
        public int videoid { get; set; }
        public object srcid { get; set; }
        public ImageVideo image { get; set; }
        public string filename { get; set; }
        public int duration { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public Meta meta { get; set; }
        public List<VideoEncoding> encodings { get; set; }
    }

    public class VideoEncoding
    {
        public int width { get; set; }
        public int height { get; set; }
        public int max_video_bitrate { get; set; }
        public string codecs { get; set; }
        public string mime { get; set; }
        public string url { get; set; }
        public string format { get; set; }
    }

    public class Meta
    {
        public int file_size_in_bytes { get; set; }
        public object total_bitrate_in_kbps { get; set; }
        public string state { get; set; }
        public object audio_codec { get; set; }
        public object audio_bitrate_in_kbps { get; set; }
        public int height { get; set; }
        public int duration_in_ms { get; set; }
        public int width { get; set; }
        public object audio_sample_rate { get; set; }
        public string format { get; set; }
        public int video_bitrate_in_kbps { get; set; }
        public object channels { get; set; }
        public string video_codec { get; set; }
        public double frame_rate { get; set; }
        public int id { get; set; }
        public object md5_checksum { get; set; }
    }

    public class Media
    {
        public string type { get; set; }
        public List<Image> data { get; set; }
        public Video videodata { get; set; }

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

  

    public class Author
    {
        public int userid { get; set; }
        public string username { get; set; }
        public object join_date { get; set; }
        public Image image { get; set; }
        public int? teamid { get; set; }
        public int reputation { get; set; }
    }

    public class flag
    {
        public string title { get; set; }
        public string flagid { get; set; }
        public string text { get; set; }
    }

    public class document
    {
        public string text { get; set; }
        public string url { get; set; }
        public int documentid { get; set; }
    }

    public class RootObject
    {
        public string conclusion_raw { get; set; }
        public string conclusion_rendered { get; set; }
        public string difficulty { get; set; }
        public List<document> documents { get; set; }
        public List<flag> flags { get; set; }
        public int guideid { get; set; }
        public Image image { get; set; }
        public string introduction_raw { get; set; }
        public string introduction_rendered { get; set; }
        public string locale { get; set; }
        public List<Part> parts { get; set; }
        public List<Prerequisite> prerequisites { get; set; }
        public List<Step> steps { get; set; }
        public string subject { get; set; }
        public string summary { get; set; }
        public string time_required { get; set; }
        public string title { get; set; }
        public List<Tool> tools { get; set; }
        public string type { get; set; }
        public int revisionid { get; set; }
        public double created_date { get; set; }
        public double published_date { get; set; }
        public double modified_date { get; set; }
        public double prereq_modified_date { get; set; }
        public bool @public { get; set; }
        public string category { get; set; }
        public string url { get; set; }
        public int patrol_threshold { get; set; }
        public bool can_edit { get; set; }
        public Author author { get; set; }
    }
}

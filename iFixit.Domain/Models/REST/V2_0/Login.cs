using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iFixit.Domain.Models.REST.V2_0.Registration.Error
{
    public class RootObject
    {
        public string message { get; set; }
    }
}
namespace iFixit.Domain.Models.REST.V2_0.Registration.Input
{
    public class RootObject
    {
        public string email { get; set; }
        public string username { get; set; }
        public string password { get; set; }
    }
}

namespace iFixit.Domain.Models.REST.V2_0.Login.Input
{
    public class RootObject
    {
        public string email { get; set; }
        public string password { get; set; }
    }
}

namespace iFixit.Domain.Models.REST.V2_0.Login.Output
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

    public class BadgeCounts
    {
        public int bronze { get; set; }
        public int silver { get; set; }
        public int gold { get; set; }
        public int total { get; set; }
    }

    public class RootObject
    {
        public string username { get; set; }
        public int userid { get; set; }
        public Image image { get; set; }
        public int reputation { get; set; }
        public List<int> teams { get; set; }
        public double? join_date { get; set; }
        public string location { get; set; }
        public int certification_count { get; set; }
        public BadgeCounts badge_counts { get; set; }
        public string summary { get; set; }
        public string about_raw { get; set; }
        public string about_rendered { get; set; }
        public string authToken { get; set; }
        //custom 
        public string email { get; set; }
    }
}
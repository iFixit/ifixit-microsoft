using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;


namespace iFixit.Domain.Models.REST
{

    public class FlatCategory
    {

        public Dictionary<string,string> Items { get; set; }

    }

    public class Category
    {

        public string Name { get; set; }
        public int Children { get; set; }
        public string UniqueId { get; set; }
        public string ImageUrl { get; set; }
        public Dictionary<string, Category> Items { get; set; }
        public int Order { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}

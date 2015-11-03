using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iFixit.Domain
{
    public class AppBase
    {
        public static AppBase Current { get; set; } = new AppBase();

        public Models.UI.Category LoadedCategories = new Models.UI.Category();
        public Models.UI.Category Categories = new Models.UI.Category();
        public Models.UI.Category Category = new Models.UI.Category();
        public string GuideId = string.Empty;
        public int CurrentPage = 0;
        public Models.UI.User User = null;
        public string SearchTerm { get; set; }

        public bool HiResApp { get; set; } = false;


        public bool ExtendeInfoApp { get; set; } = false;
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iFixit.Domain.Models;
using ServicesEngine = iFixit.Domain.Services.V2_0;
using RESTModels = iFixit.Domain.Models.REST.V2_0;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;
using iFixit.Domain.Services;
using System.Linq;
using System.Xml.Linq;

namespace iFixit.Domain.Code
{
    public static class Utils
    {

        public static async Task<ObservableCollection<Models.UI.RssItem>> LoadRss(Interfaces.IUxService _uxService, Interfaces.ISettings _settingsService)
        {

            if (_settingsService.IsConnectedToInternet())
            {
                ObservableCollection<Models.UI.RssItem> News = new ObservableCollection<Models.UI.RssItem>();
                NewsBroker nBroker = new NewsBroker();

                var xml = await nBroker.GetNews();

                var itemNodes = xml.Nodes();

                var morenodes = (from n in xml.Descendants("rss") select n).Descendants("item").Select(x => new
                Models.UI.RssItem
                {
                    Title = (string)x.Element("title")
                    ,
                    Summary = (string)x.Element("description")

                    ,
                    ImageUrl = Regex.Match(((string)(x.Element("htmlcontent"))), "<img.+?src=[\"'](.+?)[\"'].+?>", RegexOptions.IgnoreCase).Groups[1].Value

                    ,
                    Url = (string)x.Element("guid")
                    ,
                    PubDate = (DateTime)x.Element("pubDate")
                    ,
                    Author = string.Format(International.Translation.RssDateAndAuthor, (string)x.Element("creator"))
                }).ToList();


                News.Clear();
                foreach (var item in morenodes)
                {
                    item.DateString = item.PubDate.ToString("MMM dd, yyyy");
                    News.Add(item);
                }
                return News;
            }
            else
            {
                await _uxService.ShowAlert(International.Translation.NoConnection);
                return null;

            }
        }

        public static async Task DoLogOut(Interfaces.IStorage _storageService, Interfaces.IUxService _uxService, ServicesEngine.ServiceBroker Broker)
        {
            await Broker.DoLogout(AppBase.Current.User);
            AppBase.Current.User = null;
            _storageService.Delete(Constants.AUTHORIZATION);

        }

        public static ObservableCollection<Models.UI.Category> CreateBreadCrumb(Models.UI.Category selectedCategory, RESTModels.Category.RootObject selectedItem, string root)
        {

            ObservableCollection<Models.UI.Category> BreadCrumb = new ObservableCollection<Models.UI.Category>();

            selectedItem.ancestors.Reverse();

            for (int i = 1; i < selectedItem.ancestors.Count; i++)
            {
                var item = selectedItem.ancestors[i];
           
                var ancestor = selectedItem.ancestors[i - 1];
          

                string itemName = item;

                if (BreadCrumb.Any(o => o.UniqueId == ancestor))
                {
                 itemName=   itemName.Replace(ancestor + " ", "");
                }

                BreadCrumb.Add(new Models.UI.Category { Name = itemName, UniqueId = item, IndexOf = 1 });

            }
            var BreadCrumbCategoryName = root;
            if (selectedItem.ancestors.Count > 1)
            {
                for (int i = 0; i < selectedItem.ancestors.Count(); i++)
                {
                    var ancestor = selectedItem.ancestors[i];
                    if (BreadCrumb.Any(o => o.UniqueId == ancestor))
                    {
                        if (i==selectedItem.ancestors.Count-1)
                            BreadCrumbCategoryName = BreadCrumbCategoryName.Replace(BreadCrumb.Single(o=>o.UniqueId==ancestor).Name, "");
                        else

                        BreadCrumbCategoryName = BreadCrumbCategoryName.Replace(ancestor + " ", "");
                    }
                }
                
                //BreadCrumbCategoryName = BreadCrumbCategoryName;//;.TrimStart(selectedItem.ancestors[selectedItem.ancestors.Count - 1].ToCharArray());
            }

            BreadCrumb.Add(new Models.UI.Category { Name = BreadCrumbCategoryName, UniqueId = selectedCategory.UniqueId, IndexOf = 1 });


            return BreadCrumb;
        }

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        public static string StripHTML(string text)
        {
            return Regex.Replace(text, @"<(.|\n)*?>", string.Empty);
        }

        public static string GetImageNameFromUrl(string url)
        {
            return url.Substring(url.LastIndexOf('/') + 1);
        }

        public static String convertToString(this Enum eff)
        {

            return Enum.GetName(eff.GetType(), eff);

        }

        public static EnumType converToEnum<EnumType>(this String enumValue)
        {
            return (EnumType)Enum.Parse(typeof(EnumType), enumValue);
        }


        public static async Task<RESTModels.Category.RootObject> GetCategoryContent(string idCategory, iFixit.Domain.Interfaces.IStorage _storageService, ServicesEngine.ServiceBroker Broker)
        {

            Debug.WriteLine(string.Format("going for :{0}", idCategory));
            var isCategoryCached = await _storageService.Exists(Constants.CATEGORIES + idCategory, new TimeSpan(0, 12, 0, 0));
            RESTModels.Category.RootObject category = null;
            if (isCategoryCached)
            {
                var rd = await _storageService.ReadData(Constants.CATEGORIES + idCategory);
                category = rd.LoadFromJson<RESTModels.Category.RootObject>();
                Debug.WriteLine(string.Format("loaded from cache :{0}", idCategory));
            }
            else
            {
                Debug.WriteLine(string.Format("getting from service :{0}", idCategory));
                category = await Broker.GetCategory(idCategory);
                await _storageService.WriteData(Constants.CATEGORIES + idCategory,  category.SaveAsJson());
                Debug.WriteLine(string.Format("caching from service :{0}", idCategory));
            }

            // Process(category.contents_raw);
            return category;
        }

        public static string SaveAsJson(this object objectToSave)
        {
            try
            {
                if (objectToSave == null) return null;
                var st = new JsonSerializerSettings {StringEscapeHandling = StringEscapeHandling.EscapeHtml};
                return JsonConvert.SerializeObject(objectToSave, Formatting.None, st);
            }
            catch (Exception ex)
            {

                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        //public static Task<TJson> LoadFromJsonAsync<TJson>(this string jsonString)
        //{
        //    try
        //    {
        //        Debug.WriteLine("jsonString==" + jsonString);
        //        return JsonConvert.DeserializeObject<TJson>(jsonString);
        //    }
        //    catch (Exception ex)
        //    {

        //        Debug.WriteLine(ex.Message);
        //        throw;
        //    }
        //}

        public static TJson LoadFromJson<TJson>(this string jsonString)
        {
            try
            {

                Debug.WriteLine("jsonString==" + jsonString);
                return JsonConvert.DeserializeObject<TJson>(jsonString);
            }
            catch (Exception ex)
            {

                Debug.WriteLine(ex.Message);
                return default(TJson);
            }
        }
    }
}

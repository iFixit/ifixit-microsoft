using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ServicesEngine = iFixit.Domain.Services.V2_0;
using RESTModels = iFixit.Domain.Models.REST.V2_0;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;
using iFixit.Domain.Services;
using System.Xml.Linq;

namespace iFixit.Domain.Code
{
    public static class Utils
    {

        public static async Task<ObservableCollection<Models.UI.RssItem>> LoadRss(Interfaces.IUxService uxService, Interfaces.ISettings settingsService)
        {

            if (settingsService.IsConnectedToInternet())
            {
                var news = new ObservableCollection<Models.UI.RssItem>();
                var nBroker = new NewsBroker();

                var xml = await nBroker.GetNews();

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


                news.Clear();
                foreach (var item in morenodes)
                {
                    item.DateString = item.PubDate.ToString("MMM dd, yyyy");
                    news.Add(item);
                }
                return news;
            }
            else
            {
                await uxService.ShowAlert(International.Translation.NoConnection);
                return null;

            }
        }

        public static async Task DoLogOut(Interfaces.IStorage storageService, Interfaces.IUxService uxService, ServicesEngine.ServiceBroker broker)
        {
            await broker.DoLogout(AppBase.Current.User);
            AppBase.Current.User = null;
            await storageService.Delete(Constants.AUTHORIZATION);

        }

        public static ObservableCollection<Models.UI.Category> CreateBreadCrumb(Models.UI.Category selectedCategory, RESTModels.Category.RootObject selectedItem, string root)
        {

            var breadCrumb = new ObservableCollection<Models.UI.Category>();

            selectedItem.ancestors.Reverse();

            for (var i = 1; i < selectedItem.ancestors.Count; i++)
            {
                var item = selectedItem.ancestors[i];
           
                var ancestor = selectedItem.ancestors[i - 1];
          

                var itemName = item;

                if (breadCrumb.Any(o => o.UniqueId == ancestor))
                {
                 itemName=   itemName.Replace(ancestor + " ", "");
                }

                breadCrumb.Add(new Models.UI.Category { Name = itemName, UniqueId = item, IndexOf = 1 });

            }
            var breadCrumbCategoryName = root;
            if (selectedItem.ancestors.Count > 1)
            {
                for (var i = 0; i < selectedItem.ancestors.Count(); i++)
                {
                    var ancestor = selectedItem.ancestors[i];
                    if (breadCrumb.Any(o => o.UniqueId == ancestor))
                    {
                        breadCrumbCategoryName = i==selectedItem.ancestors.Count-1 ? breadCrumbCategoryName.Replace(breadCrumb.Single(o=>o.UniqueId==ancestor).Name, "") : breadCrumbCategoryName.Replace(ancestor + " ", "");
                    }
                }
                
                //BreadCrumbCategoryName = BreadCrumbCategoryName;//;.TrimStart(selectedItem.ancestors[selectedItem.ancestors.Count - 1].ToCharArray());
            }

            breadCrumb.Add(new Models.UI.Category { Name = breadCrumbCategoryName, UniqueId = selectedCategory.UniqueId, IndexOf = 1 });


            return breadCrumb;
        }

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        public static string StripHtml(string text)
        {
            return Regex.Replace(text, @"<(.|\n)*?>", string.Empty);
        }

        public static string GetImageNameFromUrl(string url)
        {
            return url.Substring(url.LastIndexOf('/') + 1);
        }

        public static string ConvertToString(this Enum eff)
        {

            return Enum.GetName(eff.GetType(), eff);

        }

        public static EnumType ConverToEnum<EnumType>(this string enumValue)
        {
            return (EnumType)Enum.Parse(typeof(EnumType), enumValue);
        }


        public static async Task<RESTModels.Category.RootObject> GetCategoryContent(string idCategory, Interfaces.IStorage storageService, ServicesEngine.ServiceBroker broker)
        {

            Debug.WriteLine($"going for :{idCategory}");
            var isCategoryCached = await storageService.Exists(Constants.CATEGORIES + idCategory, new TimeSpan(0, 12, 0, 0));
            RESTModels.Category.RootObject category;
            if (isCategoryCached)
            {
                var rd = await storageService.ReadData(Constants.CATEGORIES + idCategory);
                category = rd.LoadFromJson<RESTModels.Category.RootObject>();
                Debug.WriteLine($"loaded from cache :{idCategory}");
            }
            else
            {
                Debug.WriteLine($"getting from service :{idCategory}");
                category = await broker.GetCategory(idCategory);
                await storageService.WriteData(Constants.CATEGORIES + idCategory,  category.SaveAsJson());
                Debug.WriteLine($"caching from service :{idCategory}");
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

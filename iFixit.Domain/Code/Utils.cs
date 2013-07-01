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

namespace iFixit.Domain.Code
{
    public static class Utils
    {
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
            var isCategoryCached =  await _storageService.Exists(Constants.CATEGORIES + idCategory);
            RESTModels.Category.RootObject category = null;
            if (isCategoryCached)
            {
                var rd = await _storageService.ReadData(Constants.CATEGORIES + idCategory);
                category = rd.LoadFromJson<RESTModels.Category.RootObject>();
            }
            else
            {
                category = await Broker.GetCategory(idCategory);
                await _storageService.WriteData(Constants.CATEGORIES + idCategory, await category.SaveAsJson());
            }

            // Process(category.contents_raw);
            return category;
        }

        public static Task<string> SaveAsJson(this object objectToSave)
        {
            try
            {
                if (objectToSave == null) return null;
                var st = new JsonSerializerSettings();
                st.StringEscapeHandling = StringEscapeHandling.EscapeHtml;
                return JsonConvert.SerializeObjectAsync(objectToSave, Formatting.None, st);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        public static Task<TJson> LoadFromJsonAsync<TJson>(this string jsonString)
        {
            try
            {
                return JsonConvert.DeserializeObjectAsync<TJson>(jsonString);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw ex;
            }
        }

        public static TJson LoadFromJson<TJson>(this string jsonString)
        {
            try
            {
                var st = new JsonSerializerSettings();
                st.StringEscapeHandling = StringEscapeHandling.EscapeHtml;

                return JsonConvert.DeserializeObject<TJson>(jsonString, st);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw ex;
            }
        }
    }
}

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using iFixit.Domain.Code;
using System.IO;
using AdvancedREI.Net.Http.Compression;

namespace iFixit.Domain.Services.V2_0
{
    public class ServiceBroker
    {
        const string BaseUrl = "https://www.ifixit.com/"; //
        private int _limit = 20;
        private const string Categories = "api/2.0/categories";
        private const string Category = "api/2.0/categories/{0}";
        private const string Guides = "api/2.0/guides";
        private const string Guide = "api/2.0/guides/{0}";
        private const string Collections = "api/2.0/collections";
        private const string SEARCH_GUIDES = "api/2.0/search/{0}?filter=teardown,guide&offset={1}&limit={2}";
        private const string SEARCH_PRODUCTS = "api/2.0/search/{0}?filter={1}&offset={2}&limit={3}";
        private const string SEARCH_DEVICE = "api/2.0/search/{0}?filter={1}&offset={2}&limit={3}";
        private const string GenericSearch = "api/2.0/search/{0}?filter={1}&offset={2}&limit={3}";
        private const string Login = "api/2.0/user/token";
        private const string UserRegistration = "api/2.0/users";
        private const string Favorites = "api/2.0/users/{0}/favorites/guides";
        private const string FavoritesAction = "api/2.0/user/favorites/guides/{0}";
        private CompressedHttpClientHandler _handler = new CompressedHttpClientHandler();
        private HttpClient _client = null;




        public enum SearchFilters { Guide = 0, Teardown = 1, Wiki = 2, Category = 3, Item = 4, Info = 5, Question = 6, Product = 7, Device = 8 };

        public async Task<byte[]> GetImage(string url)
        {
            var response = await _client.GetAsync(url.ToString());
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsByteArrayAsync();
        }

        public async Task<iFixit.Domain.Models.REST.V2_0.Favorites.RootObject[]> RemoveFavorites(Models.UI.User user, string guideId)
        {
            iFixit.Domain.Models.REST.V2_0.Favorites.RootObject[] Result = null;

            try
            {
                var url = new StringBuilder();
                url.Append(BaseUrl).AppendFormat(FavoritesAction, guideId);

                AddAuthorizationHeader(user);

                var request = new HttpRequestMessage(HttpMethod.Delete, new Uri(url.ToString(), UriKind.RelativeOrAbsolute));

                var result = await _client.SendAsync(request);
                var content = await result.Content.ReadAsStringAsync();

                Debug.WriteLine(content);

            }
            catch (HttpRequestException hex)
            {
                throw hex;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return Result;
        }

        public async Task<iFixit.Domain.Models.REST.V2_0.Favorites.RootObject[]> AddFavorites(Models.UI.User user, string guideId)
        {
            iFixit.Domain.Models.REST.V2_0.Favorites.RootObject[] Result = null;

            try
            {
                var url = new StringBuilder();
                url.Append(BaseUrl).AppendFormat(FavoritesAction, guideId);
                AddAuthorizationHeader(user);
                Debug.WriteLine("sending new favorite");
                var request = new HttpRequestMessage(HttpMethod.Put, new Uri(url.ToString(), UriKind.RelativeOrAbsolute));
                var result = await _client.SendAsync(request);
                var content = await result.Content.ReadAsStringAsync();
                Debug.WriteLine(content);
                Debug.WriteLine("new favorite added");
                //Result = JsonConvert.DeserializeObject<iFixit.Domain.Models.REST.V2_0.Favorites.RootObject[]>(content);

            }
            catch (HttpRequestException hex)
            {
                throw hex;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return Result;
        }

        public async Task<iFixit.Domain.Models.REST.V2_0.Favorites.RootObject[]> GetFavorites(Models.UI.User user)
        {

            var url = new StringBuilder();
            url.Append(BaseUrl).AppendFormat(Favorites, user.UserId);
            AddAuthorizationHeader(user);

            try
            {

                return await ReturnHttpGet<iFixit.Domain.Models.REST.V2_0.Favorites.RootObject[]>(url.ToString());

            }
            catch (HttpRequestException hex)
            {
                throw hex;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public async Task<Models.REST.V2_0.Login.Output.RootObject> DoLogin(string email, string password)
        {
            var Result = new Models.REST.V2_0.Login.Output.RootObject();
            var url = new StringBuilder();
            url.Append(BaseUrl).Append(Login);
            RemoveAuthorizationHeader();
            try
            {


                var login = new Models.REST.V2_0.Login.Input.RootObject { email = email, password = password };

                var parameters =  login.SaveAsJson();

                var request = new HttpRequestMessage(HttpMethod.Post, new Uri(url.ToString(), UriKind.RelativeOrAbsolute));
                request.Content = new StreamContent(new MemoryStream(System.Text.Encoding.UTF8.GetBytes(parameters)));


                var result = await _client.SendAsync(request);
                var content = await result.Content.ReadAsStringAsync();
                Debug.WriteLine(content);
                Result = JsonConvert.DeserializeObject<Models.REST.V2_0.Login.Output.RootObject>(content);
                Result.email = email;
            }
            catch (HttpRequestException hex)
            {
                throw hex;
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return Result;
        }


        public async Task<string> DoLogout(Models.UI.User user)
        {
            var Result = string.Empty;

            try
            {
                var url = new StringBuilder();
                url.Append(BaseUrl).Append(Login);

                AddAuthorizationHeader(user);


                var request = new HttpRequestMessage(HttpMethod.Delete, new Uri(url.ToString(), UriKind.RelativeOrAbsolute));


                var result = await _client.SendAsync(request);
                var content = await result.Content.ReadAsStringAsync();

                Debug.WriteLine(content);
                Result = content;


            }
            catch (HttpRequestException hex)
            {
                throw hex;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return Result;
        }


        public async Task<Models.REST.V2_0.Login.Output.RootObject> RegistrationLogin(string email, string username, string password)
        {
            Models.REST.V2_0.Login.Output.RootObject Result = null;
            var url = new StringBuilder();
            url.Append(BaseUrl).Append(UserRegistration);
            RemoveAuthorizationHeader();
            try
            {

                var userAccount = new Models.REST.V2_0.Registration.Input.RootObject { email = email, password = password, username = username };

                var parameters =  userAccount.SaveAsJson();

                var request = new HttpRequestMessage(HttpMethod.Post,
                    new Uri(url.ToString(), UriKind.RelativeOrAbsolute))
                {
                    Content = new StreamContent(new MemoryStream(System.Text.Encoding.UTF8.GetBytes(parameters)))
                };


                var result = await _client.SendAsync(request);
                var content = await result.Content.ReadAsStringAsync();
                if (!result.IsSuccessStatusCode)
                {
                    var e = JsonConvert.DeserializeObject<Models.REST.V2_0.Registration.Error.RootObject>(content).message;
                    throw new ArgumentException(e);
                }
                Result = JsonConvert.DeserializeObject<Models.REST.V2_0.Login.Output.RootObject>(content);
                Result.email = email;
            }
            catch (HttpRequestException hex)
            {
                throw hex;
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return Result;
        }

        public async Task<Models.REST.V2_0.Collections> GetCollections()
        {
            var url = new StringBuilder();
            url.Append(BaseUrl).Append(Collections);
            RemoveAuthorizationHeader();
            try
            {

                return await ReturnHttpGet<Models.REST.V2_0.Collections>(url.ToString());

            }
            catch (HttpRequestException hex)
            {

                throw hex;
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }

        public async Task<Models.REST.V2_0.Category.RootObject> GetCategory(string uniqueId)
        {
            if (!string.IsNullOrEmpty(uniqueId))
            {
                var url = new StringBuilder();
                url.Append(BaseUrl).AppendFormat(Category, uniqueId);
                RemoveAuthorizationHeader();
                try
                {
                    return await ReturnHttpGet<Models.REST.V2_0.Category.RootObject>(url.ToString());

                }
                catch (HttpRequestException hex)
                {
                    throw new ArgumentException("HTTP");
                }
                catch (JsonException jex)
                {
                    throw new ArgumentException("Error JSON" + jex.InnerException);
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
            else
            {
                throw new ArgumentException("Missing Id for calling service");
            }



        }

        public async Task<Models.REST.V2_0.Search.Guide.RootObject> SearchGuides(string searchTerm, int currentPage)
        {

            var url = new StringBuilder();
            var offset = currentPage * _limit;
            url.Append(BaseUrl).AppendFormat(SEARCH_GUIDES, searchTerm, offset, _limit);
            RemoveAuthorizationHeader();
            try
            {
                return await ReturnHttpGet<Models.REST.V2_0.Search.Guide.RootObject>(url.ToString());

            }
            catch (HttpRequestException hex)
            {
                throw hex;
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }

        public async Task<Models.REST.V2_0.Search.Product.RootObject> SearchProducts(string searchTerm, int currentPage)
        {
            var url = new StringBuilder();
            var offset = currentPage * _limit;
            url.Append(BaseUrl).AppendFormat(SEARCH_PRODUCTS, searchTerm, SearchFilters.Product.convertToString(), offset, _limit);
            RemoveAuthorizationHeader();

            try
            {
                return await ReturnHttpGet<Models.REST.V2_0.Search.Product.RootObject>(url.ToString());

            }
            catch (HttpRequestException hex)
            {
                throw hex;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public async Task<Models.REST.V2_0.Search.Device.RootObject> SearchDevice(string searchTerm, int currentPage)
        {

            var url = new StringBuilder();
            var offset = currentPage * _limit;
            url.Append(BaseUrl).AppendFormat(SEARCH_DEVICE, searchTerm, SearchFilters.Device.convertToString(), offset, _limit);
            RemoveAuthorizationHeader();

            try
            {
                return await ReturnHttpGet<Models.REST.V2_0.Search.Device.RootObject>(url.ToString());

            }
            catch (HttpRequestException hex)
            {
                throw hex;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        private void GetChilden(JToken jto, string parentName, Models.UI.Category parentCategory)
        {

            var name = ((Newtonsoft.Json.Linq.JProperty)(jto)).Name;

            var newC = new Models.UI.Category { Name = name, Items = new System.Collections.ObjectModel.ObservableCollection<Models.UI.Category>() };

            if (jto.Values().Count() > 0)
            {
                foreach (var jt in jto.Values())
                {
                    var names = ((Newtonsoft.Json.Linq.JProperty)(jt)).Name;
                    Debug.WriteLine(name + ":" + names);

                    var children = new Models.UI.Category { UniqueId = names, Name = names, Items = new System.Collections.ObjectModel.ObservableCollection<Models.UI.Category>() };
                    parentCategory.Items.Add(children);

                    if (jt.Values().Count() > 0)
                        GetChilden(jt, name, children);

                }
            }
            else
            {

                Debug.WriteLine(parentName + ":" + name);
                parentCategory.Items.Add(newC);
            }
        }

        public async Task<Models.UI.Category> GetCategories()
        {
            var categories = new Models.UI.Category();


            try
            {
                var url = new StringBuilder();
                url.Append(BaseUrl).Append(ServiceBroker.Categories);
                RemoveAuthorizationHeader();
                var response = await _client.GetAsync(url.ToString());
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                Debug.WriteLine(content);
                var rootJson = JObject.Parse(content);

                categories.Name = "root";
                categories.Items = new System.Collections.ObjectModel.ObservableCollection<Models.UI.Category>();

                foreach (var item in rootJson)
                {
                    var itemname = item.Key;
                    var itemvalue = item.Value;


                    var newC = new Models.UI.Category { Order = RootCategoryOrder(itemname), UniqueId = itemname, Name = itemname, Items = new System.Collections.ObjectModel.ObservableCollection<Models.UI.Category>() };
                    categories.Items.Add(newC);

                    foreach (var jt in item.Value)
                    {
                        var name = ((Newtonsoft.Json.Linq.JProperty)(jt)).Name;
                        Debug.WriteLine(itemname + ":" + name);

                        var children = new Models.UI.Category { UniqueId = name, Name = name, Items = new System.Collections.ObjectModel.ObservableCollection<Models.UI.Category>() };
                        newC.Items.Add(children);
                        if (jt.Values().Count() > 0)
                            GetChilden(jt, itemname, children);


                    }



                }
            }
            catch (HttpRequestException hex)
            {
                throw hex;
            }
            catch (Exception ex)
            {

                throw ex;
            }

            var x = categories.Items.OrderBy(o => o.Order);
            var y = new Models.UI.Category();
            foreach (var catOrder in x)
            {
                y.Items.Add(catOrder);
            }

            return y;
        }

        public async Task<Models.REST.V2_0.Guide.RootObject> GetGuide(string guideId)
        {

            var url = new StringBuilder();
            url.Append(BaseUrl).AppendFormat(Guide, guideId);
            RemoveAuthorizationHeader();
            try
            {

                //  return await ReturnHTTPGet<Models.REST.V2_0.Guide.RootObject>(Url.ToString());

                Debug.WriteLine(url.ToString());
                var response = await _client.GetAsync(url.ToString());
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                Debug.WriteLine(content);


                content = content.Replace("&quot;", "''").Replace("&amp;", "&");
                //quick workarround about having the same json data object returning diferent objects
                content = content.Replace("\"type\":\"video\",\"data\"", "\"type\":\"video\",\"videodata\"");
                return content.LoadFromJson<Models.REST.V2_0.Guide.RootObject>();


            }
            catch (HttpRequestException hex)
            {
                throw hex;
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }

        private async Task<T> ReturnHttpGet<T>(string url)
        {

            try
            {
                Debug.WriteLine(url);
                var response = await _client.GetAsync(url.ToString());
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                Debug.WriteLine(content);


                // content = content.Replace("&quot;", "''").Replace("&amp;", "&");

                return content.LoadFromJson<T>();

            }
            catch (HttpRequestException hex)
            {
                throw new ArgumentException(International.Translation.ErrorHTTP, hex.Message);
            }
            catch (JsonException jex)
            {
                throw new ArgumentException("Error JSON" + jex.InnerException);
            }
            catch (Exception ex)
            {

                throw new ArgumentException(International.Translation.ErrorResponse, ex.Message);
            }

        }

        private void    RemoveAuthorizationHeader()
        {
            if (_client.DefaultRequestHeaders.Any(o => o.Key == "Authorization"))
                _client.DefaultRequestHeaders.Remove("Authorization");
        }

        private void AddAuthorizationHeader(Models.UI.User user)
        {
            if (!_client.DefaultRequestHeaders.Any(o => o.Key == "Authorization"))
                _client.DefaultRequestHeaders.Add("Authorization", string.Format("api {0}", user.Token));
        }

        private Models.UI.Category Recursive(JToken jTk, Models.UI.Category parent)
        {
            return new Models.UI.Category();
        }

        private int RootCategoryOrder(string uniqueId)
        {
            switch (uniqueId.ToLower())
            {
                case "pc":
                    return 0;
                case "electronics":
                    return 1;
                case "media player":
                    return 2;
                case "computer hardware":
                    return 3;
                case "game console":
                    return 4;
                case "car and truck":
                    return 5;
                case "appliance":
                    return 6;
                case "mac":
                    return 7;
                case "apparel":
                    return 8;
                case "phone":
                    return 9;
                case "camera":
                    return 10;
                case "vehicle":
                    return 11;
                case "tablet":
                    return 12;
                case "household":
                    return 13;
                case "skills":
                    return 14;

                default:
                    return 0;
            }
        }

        public ServiceBroker()
        {
            _client = new HttpClient();

        }

        public ServiceBroker(string appId, string appVersion)
        {
            _client = new HttpClient();
            if (!_client.DefaultRequestHeaders.Any(o => o.Key == "X-App-Id"))
                _client.DefaultRequestHeaders.Add("X-App-Id", appId);
            if (!_client.DefaultRequestHeaders.Any(o => o.Key == "User-Agent"))
                _client.DefaultRequestHeaders.Add("User-Agent", string.Format("ifixit wp+{0}",appVersion));

        }
    }
}

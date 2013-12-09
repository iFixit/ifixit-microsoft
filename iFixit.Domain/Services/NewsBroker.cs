using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;


namespace iFixit.Domain.Services
{
    public class NewsBroker
    {
        public const string Url = "http://feeds.ifixit.com/Ifixitorg?format=xml";
        private HttpClient client = new HttpClient();


        public async Task<System.Xml.Linq.XDocument> GetNews()
        {

            System.Xml.Linq.XDocument Result = null;
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/xml"));
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/rss+xml"));
            HttpResponseMessage response = await client.GetAsync(Url.ToString());
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            Debug.WriteLine(content);
            content = content.Replace("content:encoded", "htmlcontent").Replace("dc:creator","creator");
            Result = System.Xml.Linq.XDocument.Parse(content);

            return Result;
        }
    }
}

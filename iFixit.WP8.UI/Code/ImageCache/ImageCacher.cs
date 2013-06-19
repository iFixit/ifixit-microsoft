using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Media.Imaging;

namespace iFixit.WP8.UI.Code.ImageCache
{
    /// <summary>
    /// Caches web images
    /// </summary>
    public class ImageCacher
    {
        static  IsoStore s = new IsoStore();

        // Just an overload
        public static BitmapImage GetCacheImage(Uri uri)
        {
            return GetCacheImage(uri.OriginalString);
        }


        public static BitmapImage GetCacheImage(string url)
        {
            if (!url.StartsWith("http://"))
                throw new ArgumentException("ImageCacher only works with http:// images", url);

            var cacheFile = SetName(url);

            var result = new BitmapImage();
            if (s.FileExists(cacheFile))
            {
                result.SetSource(s.StreamFileFromIsoStore(cacheFile));
            }
            else
            {
                CacheImageAsync(url, result);
            }
            return result;
        }


        public static void CacheImageAsync(string url, BitmapImage image)
        {
            var items = new KeyValuePair<string, BitmapImage>(url, image);
            var t = new Thread(CacheImage);
            t.Start(items);
        }

        private static string SetName(string f)
        {
            return f.Replace("/", "_").Replace("?", "_").Replace("&", "_").Replace("=", "_").Replace("http:", "cache/");
        }

        private static void CacheImage(object input)
        {
            // extract the url and BitmapImage from our intput object
            var items = (KeyValuePair<string, BitmapImage>)input;
            var url = items.Key;
            var image = items.Value;

            var cacheFile = SetName(items.Key);

            var waitHandle = new AutoResetEvent(false);
            var fileNameAndWaitHandle = new KeyValuePair<string, AutoResetEvent>(cacheFile, waitHandle);

            var wc = new WebClient();
            wc.OpenReadCompleted += OpenReadCompleted;
            // start the caching call (web async)
            wc.OpenReadAsync(new Uri(url), fileNameAndWaitHandle);

            // wait for the file to be saved, or timeout after 5 seconds
            waitHandle.WaitOne(5000);

          

            if (s.FileExists(cacheFile))
            {
                // ok, our file now exists! set the image source on the UI thread
                Deployment.Current.Dispatcher.BeginInvoke(() => image.SetSource(s.StreamFileFromIsoStore(cacheFile)));
            }

        }

        static void OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                Debug.WriteLine(e.Error.Message);
                return;
            }

            // strip the http:// and store the image.
            var state = (KeyValuePair<string, AutoResetEvent>)e.UserState;
     
            s.SaveToIsoStore(state.Key, e.Result);
            state.Value.Set();
        }
    }

}

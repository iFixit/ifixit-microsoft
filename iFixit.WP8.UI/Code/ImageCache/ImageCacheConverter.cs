using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace iFixit.WP8.UI.Code.ImageCache
{
    public class ImageCacheConverter : IValueConverter
    {

        //http://www.ben.geek.nz/2010/07/one-time-cached-images-in-windows-phone-7/

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {



            if (value != null && value.ToString().Contains("http://"))
            {
                return ImageCacher.GetCacheImage(value.ToString());
            }

            else
            {
                return "";
            }



        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

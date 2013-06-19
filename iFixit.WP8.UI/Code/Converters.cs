using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;

namespace iFixit.WP8.UI.Code
{

    public class RandomTimeSpanGenerator : IValueConverter
    {
        readonly Random _generator = new Random();

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return TimeSpan.FromSeconds(_generator.Next(2, 5));
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }

    public class RandomIndexGenerator : IValueConverter
    {
        readonly Random _generator = new Random();

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return string.Format("/Assets/bgs/{0}.jpg", _generator.Next(1, 4));
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }

    public class TextBlockInlineConvertor : IValueConverter
    {
#if DEBUG
        // http://www.eugenedotnet.com/2011/04/binding-text-containing-tags-to-textblock-inlines-using-attached-property-in-silverlight-for-windows-phone-7/
#endif
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            var inlines = new List<Inline>();
            if (value != null)
            {
                // parse text
                var textLines =
                    value.ToString().Split(
                        new string[] { "<br/>" }
                        , StringSplitOptions.RemoveEmptyEntries);

                // add inlines and linebreaks
                foreach (string line in textLines)
                {
                    inlines.Add(new Run() { Text = line });
                    if (textLines.ToList().IndexOf(line) < textLines.Length - 1)
                    {
                        inlines.Add(new LineBreak());
                    }
                }
            }
            return inlines;
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            return value;
        }
    }

    public class TextIdentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
           object parameter, CultureInfo culture)
        {
            return new Thickness((int.Parse(value.ToString())) * 12, 0, 0, 6);
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            return value;
        }
    }

    public class UpperConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
           object parameter, CultureInfo culture)
        {
            if (value != null)
                return value.ToString().ToUpper();
            else
                return "";
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            return value;
        }
    }

    public class HomeMenuConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
           object parameter, CultureInfo culture)
        {
           
            if (value != null)
            {
                int idx = (int)value;
                if (idx != 2 || !Microsoft.Phone.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable() )
                    return
                        -1;
                else
                    return 0;
            }
            else
                return -1;
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            return value;
        }
    }

    public class LowerConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
           object parameter, CultureInfo culture)
        {
            if (value != null)
                return value.ToString().ToLower();
            else
                return "";
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            return value;
        }
    }

    public class CachingImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
           object parameter, CultureInfo culture)
        {

            if (value != null)
            {
                var imageName = Domain.Code.Utils.GetImageNameFromUrl(value.ToString());
                IsolatedStorageFile fileStorage = IsolatedStorageFile.GetUserStoreForApplication();

                return new Uri(@"isostore:/GUIDE_2099_CACHE_FOLDER/BluG5IPJGpfVFA5u.medium", UriKind.RelativeOrAbsolute);


                // return new Uri(@"ms-appdata:///local/GUIDE_2099_CACHE_FOLDER/BluG5IPJGpfVFA5u.medium", UriKind.RelativeOrAbsolute);
                //if (fileStorage.FileExists("/imagecache/" + imageName))
                //{
                //    //return @"isostore:/imagecache/" + imageName;
                //    return @"isostore:/GUIDE_{0}_CACHE_FOLDER/";
                //}
                //else
                //{
                //    return value;
                //}
            }
            // return new Thickness((int.Parse(value.ToString())) * 12, 0, 0, 6);

            return value;
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            return value;
        }
    }

    public class TextEmptyHideConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
           object parameter, CultureInfo culture)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
                return Visibility.Collapsed
                    ;
            else
                return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            return value;
        }
    }

    public class NegativeBooleanVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
           object parameter, CultureInfo culture)
        {
            if (((bool)value) == false)
                return Visibility.Visible;

            else
                return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            return value;
        }
    }

    public class ArrayBoolVisibility : Cimbalino.Phone.Toolkit.Converters.MultiValueConverterBase
    {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {

            List<bool> boolList = new List<bool>();
            bool Result = false;
            foreach (var item in values)
            {
                if (item == null)
                    boolList.Add(false);
                else
                    boolList.Add((bool)item);
            }

            string operation = parameter.ToString();

            if (operation.ToLower() == "all")
            {
                Result = boolList.TrueForAll(o => o == true);
            }
            else
            {
                Result = boolList.Any(o => o == true);
            }


            if (Result)
                return Visibility.Visible;
            else
                return Visibility.Collapsed;



        }

        public override object[] ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class ArrayBoolNegativeVisibility : Cimbalino.Phone.Toolkit.Converters.MultiValueConverterBase
    {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {

            List<bool> boolList = new List<bool>();
            bool Result = false;
            foreach (var item in values)
            {
                if (item == null)
                    boolList.Add(false);
                else
                    boolList.Add((bool)item);
            }

            string operation = parameter.ToString();

            if (operation.ToLower() == "all")
            {
                Result = boolList.TrueForAll(o => o == true);
            }
            else
            {
                Result = boolList.Any(o => o == true);
            }


            if (!Result)
                return Visibility.Visible;
            else
                return Visibility.Collapsed;



        }

        public override object[] ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }


    public class GuideMenuIndexConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
           object parameter, CultureInfo culture)
        {
            if (value == null)
                return 0;
            else
            {
                int? v = value as int?;
                if (v > 0)
                    return 1;
                else
                    return 0;
            }
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}

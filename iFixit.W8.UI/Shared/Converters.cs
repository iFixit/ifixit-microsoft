using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;

namespace iFixit.UI.Shared
{

    public sealed class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (value is bool && (bool)value) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value is Visibility && (Visibility)value == Visibility.Visible;
        }
    }

    public sealed class NegativeBooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (value is bool && !(bool)value) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value is Visibility && (Visibility)value == Visibility.Collapsed;
        }
    }

    public sealed class NegativeStringToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
                return Visibility.Visible;
            else
                if (!string.IsNullOrEmpty(value.ToString()))
                    return Visibility.Collapsed;
                else
                    return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value is Visibility && (Visibility)value == Visibility.Collapsed;
        }
    }

    public sealed class BooleanNegationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return !(value is bool && (bool)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return !(value is bool && (bool)value);
        }
    }

    public class IconVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string culture)
        {
            if (value != null)
            {
                var icon = (iFixit.Domain.Models.UI.GuideStepLineIcon)value;

                switch (icon)
                {
                    case iFixit.Domain.Models.UI.GuideStepLineIcon.Black:

                    case iFixit.Domain.Models.UI.GuideStepLineIcon.Red:

                    case iFixit.Domain.Models.UI.GuideStepLineIcon.Orange:

                    case iFixit.Domain.Models.UI.GuideStepLineIcon.Yellow:

                    case iFixit.Domain.Models.UI.GuideStepLineIcon.Green:

                    case iFixit.Domain.Models.UI.GuideStepLineIcon.Blue:

                    case iFixit.Domain.Models.UI.GuideStepLineIcon.Violet:
                        return Visibility.Collapsed;

                    case iFixit.Domain.Models.UI.GuideStepLineIcon.Icon_Note:


                    case iFixit.Domain.Models.UI.GuideStepLineIcon.Icon_reminder:

                    case iFixit.Domain.Models.UI.GuideStepLineIcon.Icon_Caution:
                        return Visibility.Visible;
                    default:
                        return Visibility.Collapsed;
                }
            }
            else
                return Visibility.Collapsed;

        }

        public object ConvertBack(object value, Type targetType, object parameter, string culture)
        {
            return value;
        }
    }

    public class IconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string culture)
        {
            if (value != null)
            {
                var icon = (iFixit.Domain.Models.UI.GuideStepLineIcon)value;

                switch (icon)
                {
                    case iFixit.Domain.Models.UI.GuideStepLineIcon.Black:

                    case iFixit.Domain.Models.UI.GuideStepLineIcon.Red:

                    case iFixit.Domain.Models.UI.GuideStepLineIcon.Orange:

                    case iFixit.Domain.Models.UI.GuideStepLineIcon.Yellow:

                    case iFixit.Domain.Models.UI.GuideStepLineIcon.Green:

                    case iFixit.Domain.Models.UI.GuideStepLineIcon.Blue:

                    case iFixit.Domain.Models.UI.GuideStepLineIcon.Violet:
                        return "/Assets/icon_no.png";

                    case iFixit.Domain.Models.UI.GuideStepLineIcon.Icon_Note:
                        return "/Assets/icon_note.png";

                    case iFixit.Domain.Models.UI.GuideStepLineIcon.Icon_reminder:
                        return "/Assets/icon_reminder.png";
                    case iFixit.Domain.Models.UI.GuideStepLineIcon.Icon_Caution:
                        return "/Assets/icon_caution.png";
                    default:
                        return "";
                }
            }
            else
                return "";

        }

        public object ConvertBack(object value, Type targetType, object parameter, string culture)
        {
            return value;
        }
    }

    public class TextBlockInlineConvertor : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, string culture)
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

        public object ConvertBack(object value, Type targetType, object parameter, string culture)
        {
            return value;
        }
    }

    public class TextIdentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string culture)
        {
            return new Thickness((int.Parse(value.ToString())) * 12, 0, 0, 6);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string culture)
        {
            return value;
        }
    }

    public class TextEmptyHideConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string culture)
        {
            if (string.IsNullOrEmpty(value.ToString()))
                return Visibility.Collapsed
                    ;
            else
                return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string culture)
        {
            return value;
        }
    }

    public class UpperConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value != null)
                return value.ToString().ToUpper();
            else
                return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value;
        }
    }

    public class SubStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {

            string Result = string.Empty;
            if (value != null)
            {
                Result = (string)value;
                int Len = int.Parse(parameter.ToString());

                if (Result.Length > Len)
                {
                    int lastSpace = Result.IndexOf(' ', Len);
                    Result =
                    Result.Substring(0, lastSpace) + "...";
                }
            }



            return Result;
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, string language)
        {
            return value;
        }
    }

}

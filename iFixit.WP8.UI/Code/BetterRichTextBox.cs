using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace iFixit.WP8.UI.Code
{

    public class BetterRichTextbox : RichTextBox
    {
        private const string UrlPattern = @"(http|ftp|https):\/\/[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?";
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(BetterRichTextbox), new PropertyMetadata(default(string), TextPropertyChanged));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }



        private static void TextPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var richTextBox = (BetterRichTextbox)dependencyObject;
            var text = (string)dependencyPropertyChangedEventArgs.NewValue;

            richTextBox.SetLinkedText(text);


        }
    }
}
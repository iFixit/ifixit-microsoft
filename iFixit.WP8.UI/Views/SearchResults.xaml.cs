using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace iFixit.WP8.UI.Views
{
    public partial class SearchResults : Code.PageBase
    {
        public SearchResults()
        {
            InitializeComponent();
        }

        //private Random _generator = new Random();
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            //BitmapImage bitmapImage = new BitmapImage(new Uri(string.Format("/Assets/bgs/{0}.jpg", _generator.Next(1, 4)), UriKind.RelativeOrAbsolute));
            //ImageBrush imageBrush = new ImageBrush();
            //imageBrush.ImageSource = bitmapImage;
            //imageBrush.Opacity = 0.5;
            //imageBrush.Stretch = Stretch.UniformToFill;

            //MainInfo.Background = imageBrush;
        }
    }
}
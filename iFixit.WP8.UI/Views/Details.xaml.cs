
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Telerik.Windows.Controls.SlideView;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using Telerik.Windows.Controls;

namespace iFixit.WP8.UI.Views
{
    public partial class Details : Code.PageBase
    {

        public Details()
        {
            InitializeComponent();

            this.Loaded += Details_Loaded;

        }

        void Details_Loaded(object sender, RoutedEventArgs e)
        {
            SetOrientation(this.Orientation);
        }

        protected override void OnOrientationChanged(OrientationChangedEventArgs e)
        {

            base.OnOrientationChanged(e);
            var orientation = e.Orientation;
            SetOrientation(orientation);
        }

        private void SetOrientation(PageOrientation orientation)
        {
            switch (orientation)
            {
                case PageOrientation.Landscape:

                case PageOrientation.LandscapeLeft:

                case PageOrientation.LandscapeRight:

                case PageOrientation.None:
                    TopMenu.Visibility = System.Windows.Visibility.Collapsed;
                    break;
                case PageOrientation.Portrait:

                case PageOrientation.PortraitDown:

                case PageOrientation.PortraitUp:
                    TopMenu.Visibility = System.Windows.Visibility.Visible;
                    break;
                default:
                    break;
            }

            switch (orientation)
            {
                case PageOrientation.Landscape:

                    break;
                case PageOrientation.LandscapeLeft:
                    ContentPanel.Margin = new Thickness(0, 0, 75, 0);
                    IndexNav.Margin = new Thickness(0, 0, 75, 0);
                    break;
                case PageOrientation.LandscapeRight:
                    ContentPanel.Margin = new Thickness(75, 0, 0, 0);
                    IndexNav.Margin = new Thickness(75, 0, 0, 0);
                    break;
                case PageOrientation.None:


                case PageOrientation.Portrait:

                case PageOrientation.PortraitDown:

                case PageOrientation.PortraitUp:


                default:
                    ContentPanel.Margin = new Thickness(0, 0, 0, 0);
                    IndexNav.Margin = new Thickness(0, 0, 0, 0);
                    break;
            }
        }

        private Random _generator = new Random();
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

            base.OnNavigatedTo(e);
        
            if (e.IsNavigationInitiator)
            {
                var VM = this.DataContext as Domain.ViewModels.Guide;
                VM.Reset();
                radWindow.IsOpen = false;
            }
        }

        private void radWindow_WindowOpened(object sender, EventArgs e)
        {
          
          //  ZoomImage. =  (this.DataContext as Domain.ViewModels.Guide).FullImagePath;
        }

        private void radSlideView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RadSlideView slideView = sender as RadSlideView;
            if (slideView != null)
            {
                var scrollViewer = ElementTreeHelper.FindVisualDescendant<ScrollViewer>(slideView.SelectedItemContainer);
                if (scrollViewer != null)
                {
                    scrollViewer.ScrollToVerticalOffset(0);
                }
            }
        }

        private void RadDataBoundListBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            var VM =this.DataContext as Domain.ViewModels.Guide;
            var item = (RadDataBoundListBox) sender;
            if (item.SelectedItem != null)
            {
              
                radWindow.IsOpen = true;  item.SelectedItem = null;
            }
        }

       
    }
}
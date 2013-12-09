using iFixit.Domain.Models.UI;
using iFixit.W81.UI.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace iFixit.W81.UI.Views
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class Guide : BasePage
    {

        private NavigationHelper navigationHelper;

        DataTransferManager dataTransferManager;
        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        void GuideDetails_Loaded(object sender, RoutedEventArgs e)
        {
            SuperImage.Width = this.ActualWidth - 240;
            SuperImage.Height = this.ActualHeight;

        }

        public Guide()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;
            this.Loaded += GuideDetails_Loaded;
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            dataTransferManager = DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested += new TypedEventHandler<DataTransferManager, DataRequestedEventArgs>(this.OnDataRequested);
            navigationHelper.OnNavigatedTo(e); base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
            dataTransferManager.DataRequested -= new TypedEventHandler<DataTransferManager, DataRequestedEventArgs>(this.OnDataRequested);
        }

        private void OnDataRequested(DataTransferManager sender, DataRequestedEventArgs e)
        {
            var VM = DataContext as Domain.ViewModels.Guide;
            e.Request.Data.Properties.Description = International.Translation.Share;
            e.Request.Data.Properties.Title = International.Translation.ShareThisGuide;
            Uri dataPackageUri = new Uri(VM.GuideUrl);
            e.Request.Data.SetWebLink(dataPackageUri);
        }

        /// <summary>
        /// Populates the page with content passed during navigation. Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session. The state will be null the first time a page is visited.</param>
        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration

        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// 
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="GridCS.Common.NavigationHelper.LoadState"/>
        /// and <see cref="GridCS.Common.NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.

     

        #endregion

        private void ZoomedOutGridView_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            //var item = (GuideBasePage)((Grid)sender).DataContext;
            //var dc = (Domain.ViewModels.Guide)this.DataContext;

            //var index = dc.Items.IndexOf(item);

            //MainGridView.ScrollIntoView(MainGridView.Items[index]);

        } 
        private void SemanticZoom_ViewChangeStarted(object sender, SemanticZoomViewChangedEventArgs e)
        {
            if (e.IsSourceZoomedInView)
            {
                MainGridView.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }
            else
            {
                MainGridView.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }
        }



        private void Grid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var item = (GuideBasePage)((Grid)sender).DataContext;
            var dc = (Domain.ViewModels.Guide)this.DataContext;

            var index = dc.Items.IndexOf(item);

            MainGridView.ScrollIntoView(MainGridView.Items[index]);
        }


        private void SuperImage_Tapped(object sender, TappedRoutedEventArgs e)
        {

            iFixit.Domain.Models.UI.GuideStepItem item = (iFixit.Domain.Models.UI.GuideStepItem)((Coding4Fun.Toolkit.Controls.SuperImage)sender).DataContext;
            ZoomImage.Visibility = Windows.UI.Xaml.Visibility.Visible;
            ImageScrollViewer.MinZoomFactor = 1;

            ImageScrollViewer.ZoomMode = ZoomMode.Enabled;
            ZoomImage.DataContext = item;
            Debug.WriteLine(item.MainImage.LargeImageUrl);
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            var x = ImageScrollViewer.ZoomFactor;
            ImageScrollViewer.ZoomToFactor(1);
            ZoomImage.Visibility = Windows.UI.Xaml.Visibility.Collapsed;

        }

        private void VideoPlay(object sender, TappedRoutedEventArgs e)
        {
            var v = (MediaElement)sender;
            if (v.CurrentState == MediaElementState.Playing)
                v.Stop();
            else
            {
                Domain.ViewModels.Guide vm = (Domain.ViewModels.Guide)this.DataContext;
                vm.IsLoading = true;
                v.Play();
            }
        }

        private void MediaElement_BufferingProgressChanged(object sender, RoutedEventArgs e)
        {
            Domain.ViewModels.Guide vm = (Domain.ViewModels.Guide)this.DataContext;
            var v = (MediaElement)sender;
            switch (v.CurrentState)
            {
                case MediaElementState.Buffering:
                    vm.IsLoading = true;
                    break;
                case MediaElementState.Closed:
                    break;
                case MediaElementState.Opening:
                    vm.IsLoading = true;
                    break;
                case MediaElementState.Paused:
                    break;
                case MediaElementState.Playing:
                    vm.IsLoading = false;
                    break;
                case MediaElementState.Stopped:
                    vm.IsLoading = false;
                    break;
                default:
                    break;
            }
        }

        private void StartImage(object sender, TappedRoutedEventArgs e)
        {
            Image i = (Image)sender;
            Domain.Models.UI.GuideStepItem gst = (Domain.Models.UI.GuideStepItem)i.DataContext;
            //   i.Visibility = Windows.UI.Xaml.Visibility.Collapsed;

            gst.PlayVideo = true;
            VideoPlayer.Source = new Uri(i.Tag.ToString());
            Video.Visibility = Windows.UI.Xaml.Visibility.Visible;

        }

        private void CloseVideoButton_Click(object sender, RoutedEventArgs e)
        {

            Video.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }
    }
}

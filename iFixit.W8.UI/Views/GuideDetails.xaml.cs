using Callisto.Controls;
using iFixit.Domain.Models.UI;
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

namespace iFixit.W8.UI.Views
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class GuideDetails : iFixit.W8.UI.Common.BasePage
    {

        DataTransferManager dataTransferManager;


        public GuideDetails()
        {
            this.InitializeComponent();

            this.Loaded += GuideDetails_Loaded;


        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            dataTransferManager = DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested += new TypedEventHandler<DataTransferManager, DataRequestedEventArgs>(this.OnDataRequested);
            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
          
            dataTransferManager.DataRequested -= new TypedEventHandler<DataTransferManager, DataRequestedEventArgs>(this.OnDataRequested);
        }

        private void OnDataRequested(DataTransferManager sender, DataRequestedEventArgs e)
        {
            var VM = DataContext as Domain.ViewModels.Guide;
            e.Request.Data.Properties.Description = International.Translation.Share;
            e.Request.Data.Properties.Title = International.Translation.ShareThisGuide;
            Uri dataPackageUri = new Uri(VM.GuideUrl);
            e.Request.Data.SetUri(dataPackageUri);
        }

        void GuideDetails_Loaded(object sender, RoutedEventArgs e)
        {
            SuperImage.Width = this.ActualWidth - 240;
            SuperImage.Height = this.ActualHeight;

        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="navigationParameter">The parameter value passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
        /// </param>
        /// <param name="pageState">A dictionary of state preserved by this page during an earlier
        /// session.  This will be null the first time a page is visited.</param>
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="pageState">An empty dictionary to be populated with serializable state.</param>
        protected override void SaveState(Dictionary<String, Object> pageState)
        {
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

        private void ZoomedOutGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ZoomedOutGridView_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            //var item = (GuideBasePage)((Grid)sender).DataContext;
            //var dc = (Domain.ViewModels.Guide)this.DataContext;

            //var index = dc.Items.IndexOf(item);

            //MainGridView.ScrollIntoView(MainGridView.Items[index]);

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
            var x =ImageScrollViewer.ZoomFactor;
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

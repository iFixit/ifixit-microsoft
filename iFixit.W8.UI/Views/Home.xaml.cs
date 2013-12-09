using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    public sealed partial class Home : iFixit.W8.UI.Common.BasePage
    {
        public Home()
        {
            this.InitializeComponent();
            Loaded += Home_Loaded;
        }

        void Home_Loaded(object sender, RoutedEventArgs e)
        {

        }


        private void SemanticZoom_ViewChangeStarted(object sender, SemanticZoomViewChangedEventArgs e)
        {
            if (e.SourceItem.Item == null) return;

            e.DestinationItem = new SemanticZoomLocation { Item = e.SourceItem.Item };
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {


            base.OnNavigatedTo(e);
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

        private void GridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GridView g = (GridView)sender;
            if (g.SelectedItems.Count > 0)
            { BottomAppBar.IsOpen = true; BottomAppBar.IsSticky = true; }
            else
            { BottomAppBar.IsOpen = false; BottomAppBar.IsSticky = false; }
        }

        private void TextBlock_Tapped(object sender, TappedRoutedEventArgs e)
        {

        }

        private void CategoriesZoom_Tapped(object sender, TappedRoutedEventArgs e)
        {
            MainGridView.ScrollIntoView(MainGridView.Items[1]);
        }

        private void FeaturedZoom_Tapped(object sender, TappedRoutedEventArgs e)
        {
            MainGridView.ScrollIntoView(MainGridView.Items[2]);
        }

        private void FavoritesZoom_Tapped(object sender, TappedRoutedEventArgs e)
        {
            MainGridView.ScrollIntoView(MainGridView.Items[3]);
        }

        private void FavoritesGV_ItemClick(object sender, ItemClickEventArgs e)
        {

        }
    }
}

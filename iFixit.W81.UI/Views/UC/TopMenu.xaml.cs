using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace iFixit.W81.UI.Views.UC
{
    public sealed partial class TopMenu : UserControl
    {
        public TopMenu()
        {
            this.InitializeComponent();
            this.Loaded += TopMenu_Loaded;
        }

        void TopMenu_Loaded(object sender, RoutedEventArgs e)
        {
            this.Focus(FocusState.Programmatic);
        }

        private void SearchBox_QuerySubmitted(SearchBox sender, SearchBoxQuerySubmittedEventArgs args)
        {
            var vm = (Domain.ViewModels.BaseViewModel)this.DataContext;
            iFixit.Domain.AppBase.Current.SearchTerm = args.QueryText;
            var previousContent = Window.Current.Content;
            var frame = previousContent as Frame;
            frame.Navigate(typeof(Views.SearchResult), args.QueryText);
            Window.Current.Content = frame;
         
        }
    }
}

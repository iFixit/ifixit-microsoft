using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace iFixit.W81.UI.Views.UC
{
    public sealed partial class TopMenuWithSubTitle : UserControl
    {
        public TopMenuWithSubTitle()
        {
            this.InitializeComponent();
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace iFixit.WP8.UI.Views
{
    public partial class About : Code.PageBase
    {
        public About()
        {
            InitializeComponent();

            this.Loaded += About_Loaded;
        }

        void About_Loaded(object sender, RoutedEventArgs e)
        {
            Domain.ViewModels.About VM = this.DataContext as Domain.ViewModels.About;

            MainInfo.SelectedIndex = VM.SelectedIndex;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using iFixit.WP8.UI.Code;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace iFixit.WP8.UI.Views
{
    public partial class HomePivot : PageBase
    {
        public HomePivot()
        {
            InitializeComponent();
        }




        /// <summary>
        /// The control doesn't support binding ... so going for an hybrid solution
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FavoritesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FavoritesList.IsSelectionEnabled)
            {
                var VM = DataContext as Domain.ViewModels.Home;

                var List = VM.SelectedFavoritesItems;
                List.Clear();
                foreach (var item in FavoritesList.SelectedItems)
                {
                    List.Add(item as iFixit.Domain.Models.UI.HomeItem);
                }

            }
        }
    }
}
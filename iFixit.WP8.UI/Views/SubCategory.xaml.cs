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
    public partial class SubCategory : Code.PageBase
    {
        public SubCategory()
        {
            InitializeComponent();
            this.Loaded += SubCategory_Loaded;
        }


        /// <summary>
        /// Workaround for binding pivot header custom templates
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void SubCategory_Loaded(object sender, RoutedEventArgs e)
        {
            Domain.ViewModels.SubCategories cat = this.DataContext as Domain.ViewModels.SubCategories;
            PivotItem p = MainPivot.Items[0] as PivotItem;
            if (cat.CategoryName.Length>8)
            p.Header = cat.CategoryName.ToLower().Substring(0,8)+"..";
            else
                p.Header = cat.CategoryName.ToLower();
         
        }



    }
}
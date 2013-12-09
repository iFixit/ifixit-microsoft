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
using System.Windows.Input;
using System.Windows.Media;

namespace iFixit.WP8.UI.Views
{
    public partial class Login : PageBase
    {
        private KeyboardHelper keyboardHelper;

        public Login()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
            this.IsTabStop = true;
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            this.keyboardHelper = new KeyboardHelper(this, LayoutRoot);
        }

       

        protected override void OnOrientationChanged(OrientationChangedEventArgs e)
        {

            base.OnOrientationChanged(e);
         

            switch (e.Orientation)
            {
                case PageOrientation.Landscape:

                    break;
                case PageOrientation.LandscapeLeft:
                    ContentPanel.Margin = new Thickness(0, 0, 75, 0);
                  
                    break;
                case PageOrientation.LandscapeRight:
                    ContentPanel.Margin = new Thickness(75, 0, 0, 0);
                  
                    break;
                case PageOrientation.None:


                case PageOrientation.Portrait:

                case PageOrientation.PortraitDown:

                case PageOrientation.PortraitUp:


                default:
                    ContentPanel.Margin = new Thickness(0, 0, 0, 0);
                 
                    break;
            }
        }

        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                keyboardHelper.HandleReturnKey(sender);
            }
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
           
        }

        private void RadPasswordBox_LostFocus(object sender, RoutedEventArgs e)
        {
            ConfirmaPassword.Focus();
        }
    }
}
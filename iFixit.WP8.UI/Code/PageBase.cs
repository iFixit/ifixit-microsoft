using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Telerik.Windows.Controls;

namespace iFixit.WP8.UI.Code
{
    public abstract class PageBase : PhoneApplicationPage
    {

        public PageBase()
        {
            InteractionEffectManager.AllowedTypes.Add(typeof(RadDataBoundListBoxItem));

       
        }

     

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var viewModel = this.DataContext as Domain.ViewModels.BaseViewModel;

            string p;
            if (this.NavigationContext.QueryString.TryGetValue("param", out p))
            {

            }
            viewModel.NavigationParameterJson = p;


            switch (e.NavigationMode)
            {
                case System.Windows.Navigation.NavigationMode.Back:
                    viewModel.NavigationType = Domain.Interfaces.NavigationModes.Back;
                    break;
                case System.Windows.Navigation.NavigationMode.Forward:
                    viewModel.NavigationType = Domain.Interfaces.NavigationModes.Forward;
                    break;
                case System.Windows.Navigation.NavigationMode.New:
                    viewModel.NavigationType = Domain.Interfaces.NavigationModes.New;
                    break;
                case System.Windows.Navigation.NavigationMode.Refresh:
                    viewModel.NavigationType = Domain.Interfaces.NavigationModes.Refresh;
                    break;
                case System.Windows.Navigation.NavigationMode.Reset:
                    viewModel.NavigationType = Domain.Interfaces.NavigationModes.Refresh;
                    break;
                default:
                    break;
            }

        }

       

        protected override void OnOrientationChanged(OrientationChangedEventArgs e)
        {
            var RootGrid = this.FindName("LayoutRoot") as Grid;
          //  var RotationGrid = RootGrid.FindName("ScreenFlow") as Grid;

            switch (e.Orientation)
            {
                case PageOrientation.Landscape:
                  //  ((Grid)this.FindName("LayoutRoot")).Margin = new Thickness(120, 0, 0, 0);
                    break;
                case PageOrientation.LandscapeLeft:
                 //   RotationGrid.Margin = new Thickness(60, 0,70, 0);
                    break;
                case PageOrientation.LandscapeRight:
                    break;
                case PageOrientation.None:
                    break;
                case PageOrientation.Portrait:
                    break;
                case PageOrientation.PortraitDown:
                    break;
                case PageOrientation.PortraitUp:
                    break;
                default:
                    break;
            }

            base.OnOrientationChanged(e);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Telerik.Windows.Controls;
using iFixit.Domain.Code;

namespace iFixit.UI.Services
{
    public class UiNavigation : Domain.Interfaces.INavigation<Domain.Interfaces.NavigationModes>
    {




        public static readonly Dictionary<Type, string> ViewModelRouting = new Dictionary<Type, string>
                                                                    {
                                                                          { typeof(Domain.ViewModels.Home), "/Views/HomePivot.xaml" }
                                                                          ,
                                                                          { typeof(Domain.ViewModels.Search), "/Views/SearchResults.xaml" }
                                                                          , 
                                                                          { typeof(Domain.ViewModels.Guide), "/Views/Details.xaml"}
                                                                          , 
                                                                          { typeof(Domain.ViewModels.About), "/Views/About.xaml" }
                                                                          , 
                                                                          { typeof(Domain.ViewModels.SubCategories), "/Views/SubCategory.xaml"}
                                                                          , 
                                                                          { typeof(Domain.ViewModels.Device), "/Views/Device.xaml"}
                                                                          ,
                                                                          {typeof(Domain.ViewModels.Login), "/Views/Login.xaml"}
                                                                          ,
                                                                          {typeof(Domain.ViewModels.Profile), "Views/Profile.xaml"}
                                                                     };




        void RootFrame_Navigated(object sender, NavigationEventArgs e)
        {



            switch (e.NavigationMode)
            {
                case System.Windows.Navigation.NavigationMode.Back:
                    NavigationType = Domain.Interfaces.NavigationModes.Back;
                    break;
                case System.Windows.Navigation.NavigationMode.Forward:
                    NavigationType = Domain.Interfaces.NavigationModes.Forward;
                    break;
                case System.Windows.Navigation.NavigationMode.New:
                    NavigationType = Domain.Interfaces.NavigationModes.New;
                    break;
                case System.Windows.Navigation.NavigationMode.Refresh:
                    NavigationType = Domain.Interfaces.NavigationModes.Refresh;
                    break;
                case System.Windows.Navigation.NavigationMode.Reset:
                    NavigationType = Domain.Interfaces.NavigationModes.Refresh;
                    break;
                default:
                    break;
            }
        }

        private Frame RootFrame => Application.Current.RootVisual as Frame;


        public bool CanGoBack => RootFrame.CanGoBack;


        //public static TJson DecodeNavigationParameter<TJson>(NavigationContext context)
        //{
        //    if (context.QueryString.ContainsKey("param"))
        //    {
        //        var param = context.QueryString["param"];
        //        return string.IsNullOrWhiteSpace(param) ? default(TJson) : param.LoadFromJson<TJson>();
        //    }

        //    throw new KeyNotFoundException();
        //}

        public void GoBack()
        {
            RootFrame.GoBack();
        }


        public void Navigate<TDestinationViewModel>(object parameter)
        {
            var navParameter = string.Empty;
            if (parameter != null)
            {
                navParameter = "?param=" + parameter.SaveAsJson();
            }

            if (!ViewModelRouting.ContainsKey(typeof (TDestinationViewModel))) return;
            var page = ViewModelRouting[typeof(TDestinationViewModel)];

            RootFrame.Navigate(new Uri("/" + page + navParameter, UriKind.Relative));
        }

        public void Navigate<TDestinationViewModel>(bool sameContext, object parameter)
        {
            if (sameContext)
            {
                ((RadPhoneApplicationFrame)this.RootFrame).Transition = new RadContinuumAndSlideTransition();
            }
            else
            {
                ((RadPhoneApplicationFrame)this.RootFrame).Transition = new RadTurnstileTransition();
            }
            Navigate<TDestinationViewModel>(parameter);
        }


        public Domain.Interfaces.NavigationModes NavigationType { get; private set; }
    }
}

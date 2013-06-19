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
                    navigationType = Domain.Interfaces.NavigationModes.Back;
                    break;
                case System.Windows.Navigation.NavigationMode.Forward:
                    navigationType = Domain.Interfaces.NavigationModes.Forward;
                    break;
                case System.Windows.Navigation.NavigationMode.New:
                    navigationType = Domain.Interfaces.NavigationModes.New;
                    break;
                case System.Windows.Navigation.NavigationMode.Refresh:
                    navigationType = Domain.Interfaces.NavigationModes.Refresh;
                    break;
                case System.Windows.Navigation.NavigationMode.Reset:
                    navigationType = Domain.Interfaces.NavigationModes.Refresh;
                    break;
                default:
                    break;
            }
        }

        private Frame RootFrame
        {
            get { return Application.Current.RootVisual as Frame; }
        }


        public bool CanGoBack
        {
            get
            {
                return RootFrame.CanGoBack;
            }
        }


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
                navParameter = "?param=" + parameter.SaveAsJson().Result;
            }

            if (ViewModelRouting.ContainsKey(typeof(TDestinationViewModel)))
            {
                var page = ViewModelRouting[typeof(TDestinationViewModel)];

                this.RootFrame.Navigate(new Uri("/" + page + navParameter, UriKind.Relative));
            }
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



        private Domain.Interfaces.NavigationModes navigationType;
        public Domain.Interfaces.NavigationModes NavigationType { get { return navigationType; } }



    }
}

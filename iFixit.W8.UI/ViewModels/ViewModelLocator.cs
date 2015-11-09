/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="using:MultiPlatform.W8.UI.ViewModels"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using GalaSoft.MvvmLight.Ioc;
using iFixit.W8.UI.Services;
using Microsoft.Practices.ServiceLocation;



namespace iFixit.W8.UI.ViewModels
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<Domain.Interfaces.IPeerConnector, iFixit.UI.Services.SimplePeerConnector>();
            SimpleIoc.Default.Register<iFixit.Domain.Interfaces.INavigation<iFixit.Domain.Interfaces.NavigationModes>, UiNavigation>();
            SimpleIoc.Default.Register<Domain.Interfaces.ISettings, UiSettings>();
            SimpleIoc.Default.Register<Domain.Interfaces.IUxService, UiUx>();
            SimpleIoc.Default.Register<Domain.Interfaces.IStorage, iFixit.Shared.UI.Services.UiStorage>();
            
            SimpleIoc.Default.Register<Domain.ViewModels.Home>();
            SimpleIoc.Default.Register<Domain.ViewModels.Guide>();
            SimpleIoc.Default.Register<Domain.ViewModels.Device>();
            SimpleIoc.Default.Register<Domain.ViewModels.Search>();
            SimpleIoc.Default.Register<Domain.ViewModels.About>();
            SimpleIoc.Default.Register<Domain.ViewModels.Login>();
            SimpleIoc.Default.Register<Domain.ViewModels.SubCategories>();
            SimpleIoc.Default.Register<Domain.ViewModels.Profile>();
        }

        public Domain.ViewModels.Home Home
        {
            get
            {
                return ServiceLocator.Current.GetInstance<Domain.ViewModels.Home>();
            }
        }

        public Domain.ViewModels.Guide Guide
        {
            get
            {
                return ServiceLocator.Current.GetInstance<Domain.ViewModels.Guide>();
            }
        }

        public Domain.ViewModels.Device Device
        {
            get
            {
                return ServiceLocator.Current.GetInstance<Domain.ViewModels.Device>();
            }
        }


        public Domain.ViewModels.Search Search
        {
            get
            {
                return ServiceLocator.Current.GetInstance<Domain.ViewModels.Search>();
            }
        }

        public Domain.ViewModels.About About
        {
            get
            {
                return ServiceLocator.Current.GetInstance<Domain.ViewModels.About>();
            }
        }

        public Domain.ViewModels.SubCategories SubCategories
        {
            get
            {
                return ServiceLocator.Current.GetInstance<Domain.ViewModels.SubCategories>();
            }
        }

        public Domain.ViewModels.Login Login
        {
            get
            {
                return ServiceLocator.Current.GetInstance<Domain.ViewModels.Login>();
            }
        }


        public Domain.ViewModels.Profile Profile
        {
            get
            {
                return ServiceLocator.Current.GetInstance<Domain.ViewModels.Profile>();
            }
        }


    
        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}
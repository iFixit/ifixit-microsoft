using GalaSoft.MvvmLight.Command;
using iFixit.Domain.Code;
using iFixit.Domain.Interfaces;
using iFixit.Domain.Models.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServicesEngine = iFixit.Domain.Services.V2_0;
using RESTModels = iFixit.Domain.Models.REST.V2_0;

namespace iFixit.Domain.ViewModels
{
    public class Device : BaseViewModel
    {

        #region "proprieties"
    
        public string DeviceId { get; set; }
        public string MainImage { get; set; }

        private string _DeviceTitle;
        public string DeviceTitle
        {
            get { return _DeviceTitle; }
            set
            {
                if (value != _DeviceTitle)
                {
                    _DeviceTitle = value;
                    NotifyPropertyChanged();
                }
            }
        }


        private SearchResultItem _SelectedItem;
        public SearchResultItem SelectedItem
        {
            get { return this._SelectedItem; }
            set
            {
                if (_SelectedItem != value)
                {
                    _SelectedItem = value;
                    NotifyPropertyChanged();
                }
            }
        }


        private ObservableCollection<Models.UI.Tool> _Tools = new ObservableCollection<Models.UI.Tool>();
        public ObservableCollection<Models.UI.Tool> Tools
        {
            get { return _Tools; }
            set
            {
                if (value != _Tools)
                {
                    _Tools = value;
                    NotifyPropertyChanged();
                }
            }
        }


        private ObservableCollection<Models.UI.IDevicePage> _DevicePages = new ObservableCollection<Models.UI.IDevicePage>();
        public ObservableCollection<Models.UI.IDevicePage> DevicePages
        {
            get { return _DevicePages; }
            set
            {
                if (value != _DevicePages)
                {
                    _DevicePages = value;
                    NotifyPropertyChanged();
                }
            }
        }
        #endregion

        #region "commands"

        private RelayCommand _GoToGuide;
        public RelayCommand GoToGuide
        {
            get
            {
                return _GoToGuide ?? (_GoToGuide = new RelayCommand(
                 () =>
                 {
                     LoadingCounter++;
                     try
                     {
                         // AppBase.Current.GuideId = SelectedItem.UniqueId;
                         _navigationService.Navigate<Guide>(true, SelectedItem.UniqueId);
                         SelectedItem = null;
                         LoadingCounter--;
                     }
                     catch (Exception ex)
                     {
                         LoadingCounter--;
                         throw ex;
                     }

                 }));
            }
        }


        private RelayCommand _Load;
        public RelayCommand Load
        {
            get
            {
                return _Load ?? (_Load = new RelayCommand(async
                 () =>
                {

                    if (NavigationType != NavigationModes.Back)
                    {
                        LoadingCounter++;
                        try
                        {
                            DevicePages.Clear();


                            var selectedCategory = this.NavigationParameter<Models.UI.Category>();

                            var selectedDevice = await Utils.GetCategoryContent(selectedCategory.Name, _storageService, Broker);

                            DevicePages.Add(new Models.UI.DeviceIntroPage
                            {
                                PageTitle = selectedDevice.display_title.ToLower(),
                                Image = selectedDevice.image != null ? selectedDevice.image.medium : "",
                                PageType = Models.UI.DevicePageType.Intro

                            });



                            var GuideTypes = (from g in selectedDevice.guides
                                              select g.type
                                                  ).Distinct().ToList();

                            foreach (var item in GuideTypes)
                            {
                                var GuidesPage = new Models.UI.DeviceListingPage { PageType = Models.UI.DevicePageType.GuideListing, PageTitle = item };

                                var filtered = selectedDevice.guides.Where(o => o.type == item).ToList();

                                foreach (var guid in filtered)
                                {
                                    var result = new Models.UI.SearchResultItem();

                                    result.Name = guid.title.Trim();
                                    result.Summary = guid.username.Trim();
                                    if (guid.image != null)
                                        result.ImageUrl = guid.image.thumbnail;
                                    result.UniqueId = guid.guideid.ToString();



                                    GuidesPage.Items.Add(result);

                                }
                                DevicePages.Add(GuidesPage);

                            }






                            LoadingCounter--;

                        }
                        catch (Exception ex)
                        {
                            LoadingCounter--;
                            throw ex;
                        }
                    }


                }));
            }
        }

        #endregion


        public Device(INavigation<Domain.Interfaces.NavigationModes> navigationService, IStorage storageService, ISettings settingsService, IUxService uxService, IPeerConnector peerConnector)
            : base(navigationService, storageService, settingsService, uxService, peerConnector)
        {


        }
    }
}

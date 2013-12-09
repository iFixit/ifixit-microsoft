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

        #region "properties"

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

        private string _CategoryBreadcrumb;
        public string CategoryBreadcrumb
        {
            get { return this._CategoryBreadcrumb; }
            set
            {
                if (_CategoryBreadcrumb != value)
                {
                    _CategoryBreadcrumb = value;
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

        private ObservableCollection<Models.UI.Category> _BreadCrumb = new ObservableCollection<Models.UI.Category>();
        public ObservableCollection<Models.UI.Category> BreadCrumb
        {
            get { return this._BreadCrumb; }
            set
            {
                if (_BreadCrumb != value)
                {
                    _BreadCrumb = value;
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

        private RelayCommand<Models.UI.SearchResultItem> _GoToGuide;
        public RelayCommand<Models.UI.SearchResultItem> GoToGuide
        {
            get
            {
                return _GoToGuide ?? (_GoToGuide = new RelayCommand<Models.UI.SearchResultItem>(
                 (item) =>
                 {
                     LoadingCounter++;
                     try
                     {

                         _navigationService.Navigate<Guide>(true, item.UniqueId);
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

        private RelayCommand<Models.UI.Category> _GoToCategory;
        public RelayCommand<Models.UI.Category> GoToCategory
        {
            get
            {
                return _GoToCategory ?? (_GoToCategory = new RelayCommand<Models.UI.Category>(
               async (selectedCategory) =>
               {

                   try
                   {
                       LoadingCounter++;

                       AppBase.Current.CurrentPage = 0;
                       var x = await Utils.GetCategoryContent(selectedCategory.UniqueId, _storageService, Broker);
                       if (x.children == null || x.children.Count == 0)
                       {
                           _navigationService.Navigate<Device>(true, selectedCategory);
                       }
                       else

                           _navigationService.Navigate<SubCategories>(true, selectedCategory);





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
                    if (_settingsService.IsConnectedToInternet())
                    {

                        if (NavigationType != NavigationModes.Back)
                        {
                            LoadingCounter++;
                            try
                            {
                                DevicePages.Clear();


                                var selectedCategory = this.NavigationParameter<Models.UI.Category>();

                                var selectedItem = await Utils.GetCategoryContent(selectedCategory.UniqueId, _storageService, Broker);

                                var GuidesPage = new Models.UI.DeviceListingPage { PageType = Models.UI.DevicePageType.GuideListing, PageTitle = iFixit.International.Translation.Guides };


                                string CategoryName = selectedItem.wiki_title;

                                /**/
                                BreadCrumb.Clear();

                                this.BreadCrumb = Utils.CreateBreadCrumb(selectedCategory, selectedItem, CategoryName);
                                CategoryBreadcrumb = BreadCrumb.Last().Name;
                                BreadCrumb.Remove(BreadCrumb.Last());

                                /**/

                                foreach (var guide in selectedItem.guides)
                                {
                                    var result = new Models.UI.SearchResultItem { IndexOf = 1, Name = guide.title.Trim().Replace("&quot;", "''"), Summary = guide.type.ToUpper() };


                                    if (guide.image != null)
                                    {
                                        result.ImageUrl = guide.image.thumbnail;
                                        result.BigImageUrl = guide.image.large;
                                    }
                                    result.UniqueId = guide.guideid.ToString();
                                    
                                    GuidesPage.Items.Add(result);

                                }

                                if (GuidesPage.Items.Count > 0)
                                    GuidesPage.HasItems = string.Empty;
                                else
                                    GuidesPage.HasItems = string.Format(International.Translation.NoGuidesOnCategoryX, CategoryName);

                                DevicePages.Add(GuidesPage);


                                DevicePages.Add(new Models.UI.DeviceIntroPage
                                {
                                    PageTitle = selectedItem.wiki_title.ToLower(),
                                    Image = selectedItem.image != null ? selectedItem.image.medium : "",
                                    PageType = Models.UI.DevicePageType.Intro,
                                    Summary = selectedItem.contents_rendered.Replace("&nbsp;&para;&nbsp;", ""),
                                    DisplayTitle = _uxService.SanetizeHTML( selectedItem.display_title)
                                });





                                LoadingCounter--;

                            }
                            catch (Exception ex)
                            {
                                LoadingCounter--;
                                throw ex;
                            }
                        }
                    }
                    else
                    {
                        await _uxService.ShowAlert(International.Translation.NoConnection);

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

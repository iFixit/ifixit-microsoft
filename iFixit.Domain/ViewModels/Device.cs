using GalaSoft.MvvmLight.Command;
using iFixit.Domain.Code;
using iFixit.Domain.Interfaces;
using iFixit.Domain.Models.UI;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using ServicesEngine = iFixit.Domain.Services.V2_0;
using RESTModels = iFixit.Domain.Models.REST.V2_0;

namespace iFixit.Domain.ViewModels
{
    public class Device : BaseViewModel
    {

        #region "properties"

        public string DeviceId { get; set; }
        public string MainImage { get; set; }

        private string _deviceTitle;
        public string DeviceTitle
        {
            get { return _deviceTitle; }
            set
            {
                if (value != _deviceTitle)
                {
                    _deviceTitle = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private string _categoryBreadcrumb;
        public string CategoryBreadcrumb
        {
            get { return _categoryBreadcrumb; }
            set
            {
                if (_categoryBreadcrumb == value) return;
                _categoryBreadcrumb = value;
                NotifyPropertyChanged();
            }
        }


        private SearchResultItem _selectedItem;
        public SearchResultItem SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (_selectedItem == value) return;
                _selectedItem = value;
                NotifyPropertyChanged();
            }
        }


        private ObservableCollection<Tool> _tools = new ObservableCollection<Tool>();
        public ObservableCollection<Tool> Tools
        {
            get { return _tools; }
            set
            {
                if (value == _tools) return;
                _tools = value;
                NotifyPropertyChanged();
            }
        }

        private ObservableCollection<Category> _breadCrumb = new ObservableCollection<Category>();
        public ObservableCollection<Category> BreadCrumb
        {
            get { return _breadCrumb; }
            set
            {
                if (_breadCrumb == value) return;
                _breadCrumb = value;
                NotifyPropertyChanged();
            }
        }


        private ObservableCollection<IDevicePage> _devicePages = new ObservableCollection<IDevicePage>();
        public ObservableCollection<IDevicePage> DevicePages
        {
            get { return _devicePages; }
            set
            {
                if (value == _devicePages) return;
                _devicePages = value;
                NotifyPropertyChanged();
            }
        }
        #endregion

        #region "commands"

        private RelayCommand<SearchResultItem> _goToGuide;
        public RelayCommand<SearchResultItem> GoToGuide
        {
            get
            {
                return _goToGuide ?? (_goToGuide = new RelayCommand<SearchResultItem>(
                 item =>
                 {
                     LoadingCounter++;
                     try
                     {

                         _navigationService.Navigate<Guide>(true, item.UniqueId);
                         SelectedItem = null;
                         LoadingCounter--;
                     }
                     catch (Exception)
                     {
                         LoadingCounter--;
                         throw;
                     }

                 }));
            }
        }

        private RelayCommand<Category> _goToCategory;
        public RelayCommand<Category> GoToCategory
        {
            get
            {
                return _goToCategory ?? (_goToCategory = new RelayCommand<Category>(
               async selectedCategory =>
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
                   catch (Exception )
                   {
                       LoadingCounter--;
                       throw;
                   }

               }));
            }

        }

        private RelayCommand _load;
        public RelayCommand Load
        {
            get
            {
                return _load ?? (_load = new RelayCommand(async
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


                                var selectedCategory = NavigationParameter<Category>();

                                var selectedItem = await Utils.GetCategoryContent(selectedCategory.UniqueId, _storageService, Broker);

                                var guidesPage = new DeviceListingPage { PageType = DevicePageType.GuideListing, PageTitle = International.Translation.Guides };


                                var categoryName = selectedItem.wiki_title;

                                /**/
                                BreadCrumb.Clear();

                                BreadCrumb = Utils.CreateBreadCrumb(selectedCategory, selectedItem, categoryName);
                                CategoryBreadcrumb = BreadCrumb.Last().Name;
                                BreadCrumb.Remove(BreadCrumb.Last());

                                /**/

                                foreach (var guide in selectedItem.guides)
                                {
                                    var result = new SearchResultItem { IndexOf = 1, Name = guide.title.Trim().Replace("&quot;", "''"), Summary = guide.type.ToUpper() };


                                    if (guide.image != null)
                                    {
                                        result.ImageUrl = guide.image.thumbnail;
                                        result.BigImageUrl = guide.image.large;
                                    }
                                    result.UniqueId = guide.guideid.ToString();
                                    
                                    guidesPage.Items.Add(result);

                                }

                                guidesPage.HasItems = guidesPage.Items.Count > 0 ? string.Empty : string.Format(International.Translation.NoGuidesOnCategoryX, categoryName);

                                DevicePages.Add(guidesPage);


                                DevicePages.Add(new DeviceIntroPage
                                {
                                    PageTitle = selectedItem.wiki_title.ToLower(),
                                    Image = selectedItem.image != null ? selectedItem.image.medium : "",
                                    PageType = DevicePageType.Intro,
                                    Summary = selectedItem.contents_rendered.Replace("&nbsp;&para;&nbsp;", ""),
                                    DisplayTitle = _uxService.SanetizeHTML( selectedItem.display_title)
                                });





                                LoadingCounter--;

                            }
                            catch (Exception )
                            {
                                LoadingCounter--;
                                throw;
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


        public Device(INavigation<NavigationModes> navigationService, IStorage storageService, ISettings settingsService, IUxService uxService, IPeerConnector peerConnector)
            : base(navigationService, storageService, settingsService, uxService, peerConnector)
        {


        }
    }
}

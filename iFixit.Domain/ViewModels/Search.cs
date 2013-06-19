using GalaSoft.MvvmLight.Command;
using iFixit.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.ObjectModel;
using iFixit.Domain.Models.REST;
using iFixit.Domain.Models.UI;


namespace iFixit.Domain.ViewModels
{
    public class Search : BaseViewModel
    {

        private string _lastSearchTerm = string.Empty;

        #region "Properties"

        protected Dictionary<Services.V1_1.ServiceBroker.SEARCH_FILTERS, Models.REST.V1_1.Search.Guide.Common> SearchSectionResult = new Dictionary<Services.V1_1.ServiceBroker.SEARCH_FILTERS, Models.REST.V1_1.Search.Guide.Common>();
        protected Dictionary<Services.V1_1.ServiceBroker.SEARCH_FILTERS, int> SearchSectionCurrentPage = new Dictionary<Services.V1_1.ServiceBroker.SEARCH_FILTERS, int>();
        protected Dictionary<Services.V1_1.ServiceBroker.SEARCH_FILTERS, bool> SearchSectionCurrentLoading = new Dictionary<Services.V1_1.ServiceBroker.SEARCH_FILTERS, bool>();

        private SearchResultItem _selectedGuide;
        public SearchResultItem SelectedGuide
        {
            get { return this._selectedGuide; }
            set
            {
                if (_selectedGuide != value)
                {
                    _selectedGuide = value;
                    NotifyPropertyChanged();
                }
            }
        }


        private SearchResultItem _SelectedDevice;
        public SearchResultItem SelectedDevice
        {
            get { return this._SelectedDevice; }
            set
            {
                if (_SelectedDevice != value)
                {
                    _SelectedDevice = value;
                    NotifyPropertyChanged();
                }
            }
        }


        private ProductItemList _SelectedProduct;
        public ProductItemList SelectedProduct
        {
            get { return this._SelectedProduct; }
            set
            {
                if (_SelectedProduct != value)
                {
                    _SelectedProduct = value;
                    NotifyPropertyChanged();
                }
            }
        }


        private string _SearchTerm = AppBase.Current.SearchTerm;
        public string SearchTerm
        {
            get { return this._SearchTerm; }
            set
            {
                if (_SearchTerm != value)
                {
                    _SearchTerm = value;
                    NotifyPropertyChanged();
                }
            }
        }


        private ObservableCollection<SearchResultItem> _Guides = new ObservableCollection<SearchResultItem>();
        public ObservableCollection<SearchResultItem> Guides
        {
            get { return this._Guides; }
            set
            {
                if (_Guides != value)
                {
                    _Guides = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private ObservableCollection<SearchResultItem> _Devices = new ObservableCollection<SearchResultItem>();
        public ObservableCollection<SearchResultItem> Devices
        {
            get { return this._Devices; }
            set
            {
                if (_Devices != value)
                {
                    _Devices = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private ObservableCollection<ProductItemList> _Products = new ObservableCollection<ProductItemList>();
        public ObservableCollection<ProductItemList> Products
        {
            get { return this._Products; }
            set
            {
                if (_Products != value)
                {
                    _Products = value;
                    NotifyPropertyChanged();
                }
            }
        }


        private string _GuidesItemsLabel;
        public string GuidesItemLabel
        {
            get { return this._GuidesItemsLabel; }
            set
            {
                if (_GuidesItemsLabel != value)
                {
                    _GuidesItemsLabel = value;
                    NotifyPropertyChanged();
                }
            }
        }



        private string _DevicesItemsLabel;
        public string DevicesItemsLabel
        {
            get { return this._DevicesItemsLabel; }
            set
            {
                if (_DevicesItemsLabel != value)
                {
                    _DevicesItemsLabel = value;
                    NotifyPropertyChanged();
                }
            }
        }



        private string _ProductsItemsLabel;
        public string ProductsItemsLabel
        {
            get { return this._ProductsItemsLabel; }
            set
            {
                if (_ProductsItemsLabel != value)
                {
                    _ProductsItemsLabel = value;
                    NotifyPropertyChanged();
                }
            }
        }


        private string _SearchQueryText;
        public string SearchQueryText
        {
            get { return this._SearchQueryText; }
            set
            {
                if (_SearchQueryText != value)
                {
                    _SearchQueryText = value;
                    NotifyPropertyChanged();
                }
            }
        }


        #endregion


        #region Commands


        private RelayCommand<ProductItemList> _GoToProduct;
        public RelayCommand<ProductItemList> GoToProduct
        {
            get
            {
                return _GoToProduct ?? (_GoToProduct = new RelayCommand<ProductItemList>(
                async (pdr) =>
                {
                    LoadingCounter++;
                    try
                    {

                        await _uxService.OpenBrowser(pdr.UniqueId);
                        SelectedProduct = null;
                    }
                    catch (Exception ex)
                    {
                        LoadingCounter--;
                        throw ex;
                    }

                }));
            }

        }

        private RelayCommand<Models.UI.SearchResultItem> _GoToDevice;
        public RelayCommand<Models.UI.SearchResultItem> GoToDevice
        {
            get
            {
                return _GoToDevice ?? (_GoToDevice = new RelayCommand<Models.UI.SearchResultItem>(
                 (SearchResultItem) =>
                 {
                     LoadingCounter++;
                     try
                     {
                         Models.UI.Category selected = new Models.UI.Category { Name = SearchResultItem.Name, UniqueId = SearchResultItem.Name };
                         _navigationService.Navigate<Device>(false, selected);
                         LoadingCounter--;
                         SelectedDevice = null;
                     }
                     catch (Exception ex)
                     {
                         LoadingCounter--;
                         throw ex;
                     }

                 }));
            }
        }

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
                         AppBase.Current.GuideId = SelectedGuide.UniqueId;
                         _navigationService.Navigate<Guide>(true, SelectedGuide.UniqueId);
                         SelectedGuide = null;
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

        private RelayCommand _DoSearch;
        public RelayCommand DoSearch
        {
            get
            {
                return _DoSearch ?? (_DoSearch = new RelayCommand(
                 async () =>
                 {
                     if (_settingsService.IsConnectedToInternet())
                     {

                         try
                         {


                             if (AppBase.Current.SearchTerm != _lastSearchTerm)
                             {
                                 ResetPageCounters();
                                 this.Guides = new ObservableCollection<SearchResultItem>();
                                 this.Devices = new ObservableCollection<SearchResultItem>();
                                 this.Products = new ObservableCollection<ProductItemList>();


                                 this._lastSearchTerm = this.SearchTerm = AppBase.Current.SearchTerm;

                                 SearchQueryText = string.Format(International.Translation.SearchedFor, this.SearchTerm);

                                 Task[] tasks = new Task[3];

                                 tasks[0] = SearchDevices();
                                 tasks[1] = SearchGuides();
                                 tasks[2] = SearchProducts();

                                 try
                                 {
                                     await Task.WhenAll(tasks);
                                 }
                                 catch (Exception ex)
                                 {

                                     throw ex;
                                 }
                             }




                             //this.CurrentPage++;
                         }
                         catch (Exception ex)
                         {
                             LoadingCounter = 0;
                             throw ex;
                         }




                     }

                     else
                     {
                         await _uxService.ShowAlert(International.Translation.NoConnection);


                     }




                 }));
            }
        }

        private RelayCommand _LoadMoreGuides;
        public RelayCommand LoadMoreGuides
        {
            get
            {
                return _LoadMoreGuides ?? (_LoadMoreGuides = new RelayCommand(
                async () =>
                {
                    if (SearchSectionCurrentPage[Services.V1_1.ServiceBroker.SEARCH_FILTERS.guide] != 0)
                        await SearchGuides();

                }));
            }
        }

        private RelayCommand _LoadMoreDevices;
        public RelayCommand LoadMoreDevices
        {
            get
            {
                return _LoadMoreDevices ?? (_LoadMoreDevices = new RelayCommand(
                async () =>
                {
                    if (SearchSectionCurrentPage[Services.V1_1.ServiceBroker.SEARCH_FILTERS.device] != 0)
                        await SearchDevices();

                }));
            }
        }

        private RelayCommand _LoadMoreProducts;
        public RelayCommand LoadMoreProducts
        {
            get
            {
                return _LoadMoreProducts ?? (_LoadMoreProducts = new RelayCommand(
                async () =>
                {
                    if (SearchSectionCurrentPage[Services.V1_1.ServiceBroker.SEARCH_FILTERS.product] != 0)
                        await SearchProducts();

                }));
            }
        }


        #endregion

        private async Task SearchGuides()
        {

            if (!SearchSectionCurrentLoading[Services.V1_1.ServiceBroker.SEARCH_FILTERS.guide])


                try
                {
                    LoadingCounter++;
                    SearchSectionCurrentLoading[Services.V1_1.ServiceBroker.SEARCH_FILTERS.guide] = true;
                    var Result = await Broker.SearchGuides(AppBase.Current.SearchTerm, SearchSectionCurrentPage[Domain.Services.V1_1.ServiceBroker.SEARCH_FILTERS.guide]);
                    this.GuidesItemLabel = string.Format("{0} ({1})", International.Translation.Guides, Result.totalResults);

                    if (Result != null)
                    {
                        foreach (var item in Result.results)
                        {
                            var newItem = new SearchResultItem
                            {
                                Name = item.title.Trim(),
                                Summary = item.type.ToUpper(),
                                ImageUrl = item.image != null ? item.image.standard : "",
                                UniqueId = item.guideid.ToString()

                            };
                            if (!Guides.Contains(newItem))
                                Guides.Add(newItem);
                        }

                        SearchSectionCurrentPage[Services.V1_1.ServiceBroker.SEARCH_FILTERS.guide] += 1;
                        SearchSectionCurrentLoading[Services.V1_1.ServiceBroker.SEARCH_FILTERS.guide] = false;
                        SearchSectionResult[Services.V1_1.ServiceBroker.SEARCH_FILTERS.guide] = (Models.REST.V1_1.Search.Guide.Common)Result;

                    }
                    LoadingCounter--;
                }
                catch (Exception ex)
                {
                    LoadingCounter--;
                    //  _uxService.ShowAlert(International.Translation.ErrorSearchDevices).RunSynchronously();
                }

        }

        private async Task SearchDevices()
        {
            if (!SearchSectionCurrentLoading[Services.V1_1.ServiceBroker.SEARCH_FILTERS.device])
                try
                {
                    LoadingCounter++;
                    var Result = await Broker.SearchDevice(AppBase.Current.SearchTerm, SearchSectionCurrentPage[Domain.Services.V1_1.ServiceBroker.SEARCH_FILTERS.device]);
                    this.DevicesItemsLabel = string.Format("{0} ({1})", International.Translation.Devices, Result.totalResults);
                    foreach (var item in Result.results)
                    {
                        var newItem = new SearchResultItem
                        {
                            Name = item.title,
                            Summary = item.summary,
                            ImageUrl = item.image != null ? item.image.standard : "",
                            UniqueId = item.url

                        };
                        if (!Devices.Contains(newItem))
                            Devices.Add(newItem);
                    }
                    SearchSectionCurrentPage[Domain.Services.V1_1.ServiceBroker.SEARCH_FILTERS.device] += 1;
                    LoadingCounter--;
                }
                catch (Exception)
                {
                    LoadingCounter--;
                    //throw ex;
                    //  _uxService.ShowAlert(International.Translation.ErrorSearchDevices).RunSynchronously();
                }


        }

        private async Task SearchProducts()
        {
            if (!SearchSectionCurrentLoading[Services.V1_1.ServiceBroker.SEARCH_FILTERS.product])

                try
                {
                    LoadingCounter++;
                    var Result = await Broker.SearchProducts(AppBase.Current.SearchTerm, SearchSectionCurrentPage[Domain.Services.V1_1.ServiceBroker.SEARCH_FILTERS.product]);
                    this.ProductsItemsLabel = string.Format("{0} ({1})", International.Translation.Devices, Result.totalResults);

                    foreach (var item in Result.results)
                    {
                        var newItem = new ProductItemList
                        {
                            Title = item.title,
                            Summary = item.text,
                            Price = item.price,
                            UniqueId = item.url

                        };
                        if (item.image != null)
                            newItem.ImageUrl = item.image.thumbnail;

                        if (!Products.Contains(newItem))
                            Products.Add(newItem);
                    }
                    SearchSectionCurrentPage[Domain.Services.V1_1.ServiceBroker.SEARCH_FILTERS.product] += 1;
                    LoadingCounter--;
                }
                catch (Exception)
                {

                    LoadingCounter--;
                    //await _uxService.ShowAlert(International.Translation.ErrorSearchDevices);
                }

        }
        
        private void ResetPageCounters()
        {
            SearchSectionCurrentPage = new Dictionary<Services.V1_1.ServiceBroker.SEARCH_FILTERS, int>();
            foreach (Services.V1_1.ServiceBroker.SEARCH_FILTERS p in Enum.GetValues(typeof(Services.V1_1.ServiceBroker.SEARCH_FILTERS)))
            {
                SearchSectionCurrentPage.Add(p, 0);
            }



            SearchSectionResult = new Dictionary<Services.V1_1.ServiceBroker.SEARCH_FILTERS, Models.REST.V1_1.Search.Guide.Common>();
            foreach (Services.V1_1.ServiceBroker.SEARCH_FILTERS p in Enum.GetValues(typeof(Services.V1_1.ServiceBroker.SEARCH_FILTERS)))
            {
                SearchSectionResult.Add(p, null);
            }
        }


        public Search(INavigation<Domain.Interfaces.NavigationModes> navigationService, IStorage storageService, ISettings settingsService, IUxService uxService, IPeerConnector peerConnector)
            : base(navigationService, storageService, settingsService, uxService, peerConnector)
        {


            foreach (Services.V1_1.ServiceBroker.SEARCH_FILTERS p in Enum.GetValues(typeof(Services.V1_1.ServiceBroker.SEARCH_FILTERS)))
            {
                SearchSectionCurrentLoading.Add(p, false);
            }

        }

    }
}

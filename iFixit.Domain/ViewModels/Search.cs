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
using iFixit.Domain.Services.V2_0;
using RESTModels = iFixit.Domain.Models.REST.V2_0;
using iFixit.Domain.Code;

namespace iFixit.Domain.ViewModels
{
    public class Search : BaseViewModel
    {

        private string _lastSearchTerm = string.Empty;

        #region "Properties"


        private string _GuidesDescription;
        public string GuidesDescription
        {
            get { return this._GuidesDescription; }
            set
            {
                if (_GuidesDescription != value)
                {
                    _GuidesDescription = value;
                    NotifyPropertyChanged();
                }
            }
        }


        private string _DevicesDescription;
        public string DevicesDescription
        {
            get { return this._DevicesDescription; }
            set
            {
                if (_DevicesDescription != value)
                {
                    _DevicesDescription = value;
                    NotifyPropertyChanged();
                }
            }
        }


        private string _ProductsDescription;
        public string ProductsDescription
        {
            get { return this._ProductsDescription; }
            set
            {
                if (_ProductsDescription != value)
                {
                    _ProductsDescription = value;
                    NotifyPropertyChanged();
                }
            }
        }


        protected Dictionary<ServiceBroker.SearchFilters, RESTModels.Search.Guide.Common> SearchSectionResult = new Dictionary<ServiceBroker.SearchFilters, RESTModels.Search.Guide.Common>();
        protected Dictionary<ServiceBroker.SearchFilters, int> SearchSectionCurrentPage = new Dictionary<ServiceBroker.SearchFilters, int>();
        protected Dictionary<ServiceBroker.SearchFilters, bool> SearchSectionCurrentLoading = new Dictionary<ServiceBroker.SearchFilters, bool>();

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

        private bool guidesMore = false;
        public bool GuidesMore
        {
            get { return guidesMore; }
            set { guidesMore = value; }
        }

        private bool devicesMore = false;
        public bool DevicesMore
        {
            get { return devicesMore; }
            set { devicesMore = value; }
        }

        private bool productsMore = false;
        public bool ProductsMore
        {
            get { return productsMore; }
            set { productsMore = value; }
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

                         _uxService.OpenIe(pdr.UniqueId);
                        SelectedProduct = null;
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

        private RelayCommand<Models.UI.SearchResultItem> _GoToDevice;
        public RelayCommand<Models.UI.SearchResultItem> GoToDevice
        {
            get
            {
                return _GoToDevice ?? (_GoToDevice = new RelayCommand<Models.UI.SearchResultItem>(
               async (SearchResultItem) =>
               {
                   LoadingCounter++;
                   try
                   {
                       if (_settingsService.IsConnectedToInternet())
                       {
                           Models.UI.Category selected = new Models.UI.Category { Name = SearchResultItem.Name, UniqueId = SearchResultItem.Name };
                           _navigationService.Navigate<Device>(false, selected);
                           LoadingCounter--;
                           SelectedDevice = null;
                       }
                       else
                       {
                           await _uxService.ShowAlert(International.Translation.NoConnection);

                       }
                   }
                   catch (Exception ex)
                   {
                       LoadingCounter--;
                       throw ex;
                   }

               }));
            }
        }

        private RelayCommand<Models.UI.SearchResultItem> _GoToCategory;
        public RelayCommand<Models.UI.SearchResultItem> GoToCategory
        {
            get
            {
                return _GoToCategory ?? (_GoToCategory = new RelayCommand<Models.UI.SearchResultItem>(
               async (c) =>
               {

                   try
                   {
                       LoadingCounter++;


                       var x = await Utils.GetCategoryContent(c.Name, _storageService, Broker);

                       var category = new Models.UI.Category
                                      {
                                          Name = x.wiki_title
                                          ,
                                          UniqueId = x.wiki_title
                                          ,
                                          IndexOf = 1
                                      };

                       if (x.children == null || x.children.Count == 0)
                       {
                           _navigationService.Navigate<Device>(true, category);
                       }
                       else

                           _navigationService.Navigate<SubCategories>(true, category);





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

        private RelayCommand _GoToGuide;
        public RelayCommand GoToGuide
        {
            get
            {
                return _GoToGuide ?? (_GoToGuide = new RelayCommand(
               async () =>
               {
                   if (_settingsService.IsConnectedToInternet())
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
                   }
                   else
                   {
                       await _uxService.ShowAlert(International.Translation.NoConnection);

                   }
               }));
            }
        }
        private RelayCommand<SearchResultItem> _GoToGuideItem;
        public RelayCommand<SearchResultItem> GoToGuideItem
        {
            get
            {
                return _GoToGuideItem ?? (_GoToGuideItem = new RelayCommand<SearchResultItem>(
               async (guide) =>
               {
                   if (_settingsService.IsConnectedToInternet())
                   {
                       LoadingCounter++;
                       try
                       {
                           AppBase.Current.GuideId = guide.UniqueId;
                           _navigationService.Navigate<Guide>(true, guide.UniqueId);
                           SelectedGuide = null;
                           LoadingCounter--;
                       }
                       catch (Exception ex)
                       {
                           LoadingCounter--;
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
                                 DevicesMore = ProductsMore = GuidesMore = true;
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
                    await MoreGuides();

                }));
            }
        }

        public async Task MoreGuides()
        {
            if (_settingsService.IsConnectedToInternet())
            {

                if (SearchSectionCurrentPage[ServiceBroker.SearchFilters.Guide] != 0)
                    await SearchGuides();
            }

            else
            {
                await _uxService.ShowAlert(International.Translation.NoConnection);


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
                    await MoreDevices();

                }));
            }
        }

        public async Task MoreDevices()
        {
            if (_settingsService.IsConnectedToInternet())
            {
                if (SearchSectionCurrentPage[ServiceBroker.SearchFilters.Device] != 0)
                    await SearchDevices();
            }

            else
            {
                await _uxService.ShowAlert(International.Translation.NoConnection);


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
                    await MoreProducts();
                }));
            }
        }

        public async Task MoreProducts()
        {
            if (_settingsService.IsConnectedToInternet())
            {
                if (SearchSectionCurrentPage[ServiceBroker.SearchFilters.Product] != 0)
                    await SearchProducts();
            }

            else
            {
                await _uxService.ShowAlert(International.Translation.NoConnection);


            }
        }


        #endregion

        private async Task SearchGuides()
        {

            if (!SearchSectionCurrentLoading[ServiceBroker.SearchFilters.Guide])
                if (GuidesMore)

                    try
                    {
                        LoadingCounter++;
                        SearchSectionCurrentLoading[ServiceBroker.SearchFilters.Guide] = true;
                        var Result = await Broker.SearchGuides(AppBase.Current.SearchTerm, SearchSectionCurrentPage[ServiceBroker.SearchFilters.Guide]);
                        this.GuidesItemLabel = string.Format("{0} ({1})", International.Translation.Guides, Result.totalResults);
                        this.GuidesMore = Result.moreResults;
                        if (Result != null)
                        {
                            foreach (var item in Result.results)
                            {
                                var newItem = new SearchResultItem
                                {
                                    Name = item.title.Trim().Replace("&quot;", "''"),
                                    Summary = item.type.ToUpper(),
                                    ImageUrl = item.image != null ? item.image.standard : "",
                                    UniqueId = item.guideid.ToString()

                                };
                                if (!Guides.Contains(newItem))
                                    Guides.Add(newItem);
                            }

                            SearchSectionCurrentPage[ServiceBroker.SearchFilters.Guide] += 1;
                            SearchSectionCurrentLoading[ServiceBroker.SearchFilters.Guide] = false;
                            SearchSectionResult[ServiceBroker.SearchFilters.Guide] = (RESTModels.Search.Guide.Common)Result;

                            if (Result.totalResults == 0)
                                GuidesDescription = International.Translation.NoGuidesFound;
                            else
                                GuidesDescription = string.Empty;
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

            if (!SearchSectionCurrentLoading[ServiceBroker.SearchFilters.Device])
                if (DevicesMore)
                    try
                    {
                        LoadingCounter++;
                        var Result = await Broker.SearchDevice(AppBase.Current.SearchTerm
                            , SearchSectionCurrentPage[ServiceBroker.SearchFilters.Device]);
                        this.DevicesItemsLabel = string.Format("{0} ({1})", International.Translation.Devices, Result.totalResults);
                        DevicesMore = Result.moreResults;



                        foreach (var item in Result.results)
                        {
                            var newItem = new SearchResultItem
                            {
                                Name = item.title,
                                Summary = item.summary,
                                ImageUrl = item.image != null ? item.image.standard : "",
                                UniqueId = item.url

                            };
                            if (!Devices.Any(o => o.UniqueId == newItem.UniqueId))
                                Devices.Add(newItem);
                            else
                                throw new ArgumentException("item repetido");
                        }
                        SearchSectionCurrentPage[ServiceBroker.SearchFilters.Device] += 1;

                        if (Result.totalResults == 0)
                            DevicesDescription = International.Translation.NoDevicesFound;
                        else
                            DevicesDescription = string.Empty;


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
            if (!SearchSectionCurrentLoading[ServiceBroker.SearchFilters.Product])
                if (ProductsMore)
                    try
                    {
                        LoadingCounter++;
                        var Result = await Broker.SearchProducts(AppBase.Current.SearchTerm, SearchSectionCurrentPage[ServiceBroker.SearchFilters.Product]);
                        this.ProductsItemsLabel = string.Format("{0} ({1})", International.Translation.Devices, Result.totalResults);
                        this.ProductsMore = Result.moreResults;
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
                                newItem.ImageUrl = item.image.thumbnail.Replace(".thumbnail", ".standard");

                            if (!Products.Contains(newItem))
                                Products.Add(newItem);
                        }
                        SearchSectionCurrentPage[ServiceBroker.SearchFilters.Product] += 1;

                        if (Result.totalResults == 0)
                            ProductsDescription = International.Translation.NoProductsFound;
                        else
                            ProductsDescription = string.Empty;
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
            SearchSectionCurrentPage = new Dictionary<ServiceBroker.SearchFilters, int>();
            foreach (ServiceBroker.SearchFilters p in Enum.GetValues(typeof(ServiceBroker.SearchFilters)))
            {
                SearchSectionCurrentPage.Add(p, 0);
            }



            SearchSectionResult = new Dictionary<ServiceBroker.SearchFilters, RESTModels.Search.Guide.Common>();
            foreach (ServiceBroker.SearchFilters p in Enum.GetValues(typeof(ServiceBroker.SearchFilters)))
            {
                SearchSectionResult.Add(p, null);
            }
        }


        public Search(INavigation<Domain.Interfaces.NavigationModes> navigationService, IStorage storageService, ISettings settingsService, IUxService uxService, IPeerConnector peerConnector)
            : base(navigationService, storageService, settingsService, uxService, peerConnector)
        {


            foreach (ServiceBroker.SearchFilters p in Enum.GetValues(typeof(Services.V2_0.ServiceBroker.SearchFilters)))
            {
                SearchSectionCurrentLoading.Add(p, false);
            }

        }

    }
}

using GalaSoft.MvvmLight.Command;
using iFixit.Domain.Code;
using iFixit.Domain.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using ServicesEngine = iFixit.Domain.Services.V2_0;
using RESTModels = iFixit.Domain.Models.REST.V2_0;

namespace iFixit.Domain.ViewModels
{
    public class Home : BaseViewModel
    {



        #region "Properties




        private string _favoritesDescription = International.Translation.LoginToViewYourFavorites;
        public string FavoritesDescription
        {
            get { return _favoritesDescription; }
            set
            {
                if (_favoritesDescription == value) return;
                _favoritesDescription = value;
                NotifyPropertyChanged();
            }
        }




        private ObservableCollection<Models.UI.Category> _categories = new ObservableCollection<Models.UI.Category>();
        public ObservableCollection<Models.UI.Category> Categories
        {
            get { return _categories; }
            set
            {
                if (_categories == value) return;
                _categories = value;
                NotifyPropertyChanged();
            }
        }


        private string _mainCollectionImage;
        public string MainCollectionImage
        {
            get { return _mainCollectionImage; }
            set
            {
                if (_mainCollectionImage == value) return;
                _mainCollectionImage = value;
                NotifyPropertyChanged();
            }
        }


        private string _mainCollectionName;
        public string MainCollectionName
        {
            get { return _mainCollectionName; }
            set
            {
                if (_mainCollectionName == value) return;
                _mainCollectionName = value;
                NotifyPropertyChanged();
            }
        }


        private Models.UI.RssItem _mainNews = new Models.UI.RssItem();
        public Models.UI.RssItem MainNews
        {
            get { return _mainNews; }
            set
            {
                if (_mainNews == value) return;
                _mainNews = value;
                NotifyPropertyChanged();
            }
        }
        

        private ObservableCollection<Models.UI.RssItem> _news = new ObservableCollection<Models.UI.RssItem>();
        public ObservableCollection<Models.UI.RssItem> News
        {
            get { return _news; }
            set
            {
                if (_news == value) return;
                _news = value;
                NotifyPropertyChanged();
            }
        }

        private string _mainCollectionDescriptions;
        public string MainCollectionDescriptions
        {
            get { return _mainCollectionDescriptions; }
            set
            {
                if (_mainCollectionDescriptions == value) return;
                _mainCollectionDescriptions = value;
                NotifyPropertyChanged();
            }
        }


        bool _selectionMode;
        public bool SelectionMode
        {
            get { return _selectionMode; }
            set
            {
                if (_selectionMode == value) return;
                _selectionMode = value;
                NotifyPropertyChanged();
            }
        }

        Models.UI.Category _selectedCategory;
        public Models.UI.Category SelectedCategory
        {
            get { return _selectedCategory; }
            set
            {
                if (_selectedCategory == value) return;
                _selectedCategory = value;
                NotifyPropertyChanged();
            }
        }


        private Models.UI.HomeItem _selectedGuide;
        public Models.UI.HomeItem SelectedGuide
        {
            get { return _selectedGuide; }
            set
            {
                if (_selectedGuide == value) return;
                _selectedGuide = value;
                NotifyPropertyChanged();
            }
        }


         ObservableCollection<Models.UI.HomeItem> _collectionsItems = new ObservableCollection<Models.UI.HomeItem>();
        public ObservableCollection<Models.UI.HomeItem> CollectionsItems
        {
            get { return _collectionsItems; }
            set
            {
                if (_collectionsItems == value) return;
                _collectionsItems = value;
                NotifyPropertyChanged();
            }
        }


         bool _hasFavorites;
        public bool HasFavorites
        {
            get { return _hasFavorites; }
            set
            {
                if (_hasFavorites == value) return;
                _hasFavorites = value;
                NotifyPropertyChanged();
            }
        }



         bool _isLoggedIn;
        public bool IsLoggedIn
        {
            get { return _isLoggedIn; }
            set
            {
                if (_isLoggedIn == value) return;
                _isLoggedIn = value;
                NotifyPropertyChanged();
            }
        }
        

        private ObservableCollection<object> _selectedFavoritesItems = new ObservableCollection<object>();
        public ObservableCollection<object> SelectedFavoritesItems
        {
            get { return _selectedFavoritesItems; }
            set
            {

                _selectedFavoritesItems = value;

                SelectionMode = _selectedFavoritesItems.Count > 0;
                NotifyPropertyChanged();

            }
        }

        private ObservableCollection<Models.UI.HomeItem> _favoritesItems = new ObservableCollection<Models.UI.HomeItem>();
        public ObservableCollection<Models.UI.HomeItem> FavoritesItems
        {
            get { return _favoritesItems; }
            set
            {
                if (_favoritesItems == value) return;
                _favoritesItems = value;
                HasFavorites = _favoritesItems.Count > 0;
                NotifyPropertyChanged();
            }
        }


        private int _nextCollection;
        public int NextCollection
        {
            get { return _nextCollection; }
            set
            {
                if (_nextCollection == value) return;
                _nextCollection = value;
                NotifyPropertyChanged();
            }
        }




        
        
        


        #endregion

        #region Commands



        private RelayCommand<int> _goToNextCollection;
        public RelayCommand<int> GoToNextCollection
        {
            get
            {
                return _goToNextCollection ?? (_goToNextCollection = new RelayCommand<int>(
                 async param =>
                 {

                     if (_settingsService.IsConnectedToInternet())
                     {
                         LoadingCounter++;
                         CollectionsItems.Clear();
                         var resultsCollections = await GetCollections(param);
                         var guidesToLoad = new Task[resultsCollections[param].guides.Count()];

                         for (var i = 0; i < resultsCollections[param].guides.Count(); i++)
                         {
                             guidesToLoad[i] = GetGuide(resultsCollections[param].guides[i].guideid.ToString());

                         }

                         await Task.WhenAll(guidesToLoad);

                         LoadingCounter--;
                     }
                     else
                     {
                         await _uxService.ShowAlert(International.Translation.NoConnection);

                     }
                 }));
            }
        }



        private RelayCommand _deleteFavorites;
        public RelayCommand DeleteFavorites
        {
            get
            {
                return _deleteFavorites ?? (_deleteFavorites = new RelayCommand(
                 async () =>
                 {
                     LoadingCounter++;

                     var toDelete = new Task[SelectedFavoritesItems.Count];
                     var foldersToDelete = new Task[SelectedFavoritesItems.Count];
                     for (var i = 0; i < SelectedFavoritesItems.Count; i++)
                     {
                         var item = SelectedFavoritesItems[i] as Models.UI.HomeItem;
                         if (item == null) continue;
                         toDelete[i] = Broker.RemoveFavorites(AppBase.Current.User, item.UniqueId);
                         foldersToDelete[i] = _storageService.RemoveFolder(string.Format(Constants.GUIDE_CACHE_FOLDER, item.UniqueId));
                     }

                     await Task.WhenAll(toDelete);
                     await Task.WhenAll(foldersToDelete);
                     await GetUserFavorites();

                     SelectionMode = !SelectionMode;
                     LoadingCounter--;
                 }));
            }
        }

        private RelayCommand _setSelectionMode;
        public RelayCommand SetSelectionMode
        {
            get
            {
                return _setSelectionMode ?? (_setSelectionMode = new RelayCommand(
                 () =>
                 {
                     if (FavoritesItems.Count == 0)
                     {
                         _navigationService.Navigate<Login>(false);
                     }
                     else
                     {
                         SelectionMode = !SelectionMode;
                     }


                 }));
            }
        }

        private RelayCommand _clearSelectedItems;
        public RelayCommand ClearSelectedItems
        {
            get
            {
                return _clearSelectedItems ?? (_clearSelectedItems = new RelayCommand(
                 () =>
                 {
                     SelectedFavoritesItems.Clear();


                 }));
            }
        }

        private RelayCommand _selectAllSelectedItems;
        public RelayCommand SelectAllSelectedItems
        {
            get
            {
                return _selectAllSelectedItems ?? (_selectAllSelectedItems = new RelayCommand(
                 () =>
                 {
                     foreach (var item in FavoritesItems)
                     {
                         SelectedFavoritesItems.Add(item);
                     }


                 }));
            }
        }


        private RelayCommand<Models.UI.HomeItem> _tapFavorite;
        public RelayCommand<Models.UI.HomeItem> TapFavorite
        {
            get
            {
                return _tapFavorite ?? (_tapFavorite = new RelayCommand<Models.UI.HomeItem>(
                 selectedGuide =>
                 {

                     try
                     {

                         LoadingCounter++;

                         if (!SelectionMode)
                         {
                             AppBase.Current.GuideId = selectedGuide.UniqueId;
                             _navigationService.Navigate<Guide>(true, selectedGuide.UniqueId);
                             selectedGuide = null;
                             LoadingCounter--;
                         }
                         else
                         {
                             // Edit List

                         }
                         LoadingCounter--;
                     }
                     catch (Exception )
                     {
                         LoadingCounter--;
                         throw ;
                     }

                 }));
            }
        }



        private RelayCommand _getFavorites;
        public RelayCommand GetFavorites
        {
            get
            {
                return _getFavorites ?? (_getFavorites = new RelayCommand(
                async () =>
                {

                    try
                    {


                        await GetUserFavorites();

                    }
                    catch (Exception)
                    {
                        LoadingCounter--;
                        throw;
                    }

                }));
            }
        }


        private RelayCommand<Models.UI.HomeItem> _goToGuide;
        public RelayCommand<Models.UI.HomeItem> GoToGuide
        {
            get
            {
                return _goToGuide ?? (_goToGuide = new RelayCommand<Models.UI.HomeItem>(
             async selectedGuide =>
             {

                 try
                 {
                     if (_settingsService.IsConnectedToInternet())
                     {
                         LoadingCounter++;
                         AppBase.Current.GuideId = selectedGuide.UniqueId;
                         _navigationService.Navigate<Guide>(true, selectedGuide.UniqueId);
                         selectedGuide = null;
                         LoadingCounter--;
                     }
                     else
                     {
                         await _uxService.ShowAlert(International.Translation.NoConnection);

                     }
                 }
                 catch (Exception)
                 {
                     LoadingCounter--;
                     throw;
                 }

             }));
            }
        }


        private RelayCommand<Models.UI.Category> _goToCategory;
        public RelayCommand<Models.UI.Category> GoToCategory
        {
            get
            {
                return _goToCategory ?? (_goToCategory = new RelayCommand<Models.UI.Category>(
                async selectedCategory =>
                {

                    try
                    {
                        if (_settingsService.IsConnectedToInternet())
                        {
                            LoadingCounter++;
                            AppBase.Current.CurrentPage = 0;
                            SelectedCategory = null;
                            _navigationService.Navigate<SubCategories>(true, selectedCategory);
                            LoadingCounter--;
                        }
                        else
                        {
                            await _uxService.ShowAlert(International.Translation.NoConnection);

                        }
                    }
                    catch (Exception)
                    {
                        LoadingCounter--;
                        throw;
                    }

                }));
            }

        }


        private RelayCommand _loadHome;
        public RelayCommand LoadHome
        {

            get
            {
                return _loadHome ?? (_loadHome = new RelayCommand(async
                    () =>
                {
                    await LoadUser();

                    var isStart = NavigationParameter<string>();

                    var firstLoad = false;

                    switch (NavigationType)
                    {
                        case NavigationModes.New:
                            if (string.IsNullOrEmpty(isStart))
                                firstLoad = true;
                            else
                            {
                                LoadingCounter++;
                                await GetUserFavorites();
                                LoadingCounter--;
                            }
                            break;
                        case NavigationModes.Forward:
                            break;
                        case NavigationModes.Refresh:
                        case NavigationModes.Back:
                            LoadingCounter++;
                            await GetUserFavorites();
                            LoadingCounter--;

                            break;
                        case NavigationModes.Reset:
                            break;
                        default:
                            break;
                    }



                    if (firstLoad)
                    {

                        if (_settingsService.IsConnectedToInternet())
                        {

                            firstLoad = false;
                            try
                            {


                                Models.REST.V2_0.Collections collections = null;

                                // must do paralel ....
                                // var _ResultsCategory = GetCategories(Result);
                                var resultsCollections = GetCollections(0);
                                var resultLogin = GetUserFavorites();
                                await Task.WhenAll(resultsCollections, resultLogin);

                                //Result = _ResultsCategory.Result;
                                //TODO: New UI
                                //Categories = Result.Items;
                                collections = resultsCollections.Result;


                                var howManyCallsToDo = collections[0].guides.Count() + Categories.Count;
                                var guidesToLoad = new Task[collections[0].guides.Count()];
                                var categoriesToLoad = new Task[Categories.Count];



                                for (var i = 0; i < Categories.Count; i++)
                                {
                                    categoriesToLoad[i] = GetCategoryContent(Categories[i].Name);

                                }

                                for (var i = 0; i < collections[0].guides.Count(); i++)
                                {
                                    var item = collections[0].guides[i].guideid;
                                    if (CollectionsItems.All(o => o.UniqueId != item.ToString()))
                                        CollectionsItems.Add(new Models.UI.HomeItem
                                        {
                                            UniqueId = item.ToString(),
                                            IndexOf = i
                                        });
                                    guidesToLoad[i] = GetGuide(item.ToString());

                                }

                                if (AppBase.Current.ExtendeInfoApp)
                                {
                                    News = await Utils.LoadRss(_uxService, _settingsService);
                                    if (News.Count > 0)
                                        MainNews = News[0];

                                }
                                await Task.WhenAll(guidesToLoad);
                                await Task.WhenAll(categoriesToLoad);


                              

                            }
                            catch (Exception)
                            {
                                LoadingCounter--;

                            }
                        }
                        else
                        {
                            await _uxService.ShowAlert(International.Translation.NoConnection);

                            LoadingCounter++;
                            await GetUserFavorites();
                            LoadingCounter--;
                        }
                    }


                    /**/

                }

                ));
            }

        }






        #endregion

        private async Task<Models.REST.V2_0.Collections> GetCollections(int idx)
        {

            try
            {
                RESTModels.Collections collections;
                LoadingCounter++;
                var isCollectionsCached = await _storageService.Exists(Constants.COLLECTIONS, new TimeSpan(0, 12, 0, 0));
                if (isCollectionsCached)
                {
                    var rd = await _storageService.ReadData(Constants.COLLECTIONS);
                    collections = rd.LoadFromJson<RESTModels.Collections>();
                }
                else
                {
                    collections = await Broker.GetCollections();
                    await _storageService.WriteData(Constants.COLLECTIONS,  collections.SaveAsJson());

                }

                NextCollection = idx + 1;
                if (NextCollection >= collections.Count)
                {
                    NextCollection = 0;
                }

                MainCollectionImage = collections[idx].image.standard;
                MainCollectionName = collections[idx].title;
                MainCollectionDescriptions = Utils.UnixTimeStampToDateTime(collections[idx].date).ToString("MMM d, yyyy");
                LoadingCounter--;
                return collections;
            }

            catch (Exception ex)
            {
                _uxService.ShowAlert(string.Format(International.Translation.ErrorGetting, "collections", ex.Message)).RunSynchronously();
                LoadingCounter--;
                throw ;
            }

        }


        private async Task LoadLocalFavories()
        {
            LoadingCounter++;
            var localFavorites = await _storageService.Exists(Constants.FAVORITES);
            if (localFavorites)
            {

                BindFavorites((await _storageService.ReadData(Constants.FAVORITES)).LoadFromJson<RESTModels.Favorites.RootObject[]>());
            }
            LoadingCounter--;
        }

        private async Task LoadUser()
        {
            LoadingCounter++;
            if (await _storageService.Exists(Constants.AUTHORIZATION))
            {
                var f = await _storageService.ReadData(Constants.AUTHORIZATION);
                var x = f.LoadFromJson<RESTModels.Login.Output.RootObject>();

                BindAuthentication(x);


            }
            LoadingCounter--;
        }

        private async Task GetUserFavorites()
        {
            LoadingCounter++;
            if (AppBase.Current.User != null)
            {
                IsLoggedIn = true;
                if (_settingsService.IsConnectedToInternet())
                {
                    var r = await Broker.GetFavorites(AppBase.Current.User);
                    await _storageService.WriteData(Constants.FAVORITES,  r.SaveAsJson());
                    BindFavorites(r);
                }
                else
                {

                    await LoadLocalFavories();
                }
            }
            else
            {
                 IsLoggedIn=false;
                FavoritesDescription = International.Translation.LoginToViewYourFavorites;
                FavoritesItems.Clear();
            }
            LoadingCounter--;
        }

        private void BindFavorites(RESTModels.Favorites.RootObject[] r)
        {
            FavoritesItems.Clear();
            HasFavorites = false;

            if (r != null && r.Any())
            {
                FavoritesDescription = string.Empty;
                HasFavorites = true;
                foreach (var item in r.Where(item => FavoritesItems.All(o => o.UniqueId != item.guide.guideid.ToString())))
                {
                    FavoritesItems.Add(new Models.UI.HomeItem
                    {
                        Name = item.guide.title
                        ,
                        UniqueId = item.guide.guideid.ToString()
                        ,
                        ImageUrl = item.guide.image != null ? item.guide.image.standard : ""
                        ,
                        BigImageUrl = item.guide.image != null ? item.guide.image.large : ""
                        ,
                        IndexOf = 1
                    });
                }
            }
            else
            {
                FavoritesDescription = International.Translation.NoFavorites;
            }

        }

        private async Task GetGuide(string idGuide)
        {
            LoadingCounter++;
            var cachedItemName = string.Format(Constants.GUIDE, idGuide);
            Debug.WriteLine($"going for guide :{idGuide}");
            var isCategoryCached = await _storageService.Exists(cachedItemName, new TimeSpan(5, 0, 0, 0));

            RESTModels.Guide.RootObject guide = null;
            if (isCategoryCached)
            {
                Debug.WriteLine($"going for cached :{idGuide}");
                var rd = await _storageService.ReadData(cachedItemName);
                Debug.WriteLine($"cached :{idGuide} : {rd}");
                guide = rd.LoadFromJson<RESTModels.Guide.RootObject>();
            }
            else
            {
                Debug.WriteLine($"Calling Guide :{idGuide}");
                guide = await Broker.GetGuide(idGuide);
                Debug.WriteLine($"Saving Guide :{idGuide}");
                await _storageService.WriteData(cachedItemName,  guide.SaveAsJson());
            }


            var itemtoUpdate = CollectionsItems.SingleOrDefault(o => o.UniqueId == idGuide);
            if (itemtoUpdate != null)
            {
                itemtoUpdate.Name = guide.title;
                itemtoUpdate.ImageUrl = guide.image.standard;
                itemtoUpdate.BigImageUrl = guide.image.large;

            }
            else
            {
                //throw new ArgumentException("Where's the guide?" + idGuide);
            }


            //if (CollectionsItems.Where(o => o.UniqueId == newCollectionItem.UniqueId).SingleOrDefault() == null)
            //    CollectionsItems.Add(newCollectionItem);
            LoadingCounter--;

        }

        private async Task GetCategoryContent(string idCategory)
        {

            LoadingCounter++;
            Debug.WriteLine($"going for category :{idCategory}");
            var isCategoryCached = await _storageService.Exists(Constants.CATEGORIES + idCategory, new TimeSpan(5, 0, 0, 0));
            RESTModels.Category.RootObject category = null;
            if (isCategoryCached)
            {
                Debug.WriteLine($"get from cache category :{idCategory}");
                var rd = await _storageService.ReadData(Constants.CATEGORIES + idCategory);
                category = rd.LoadFromJson<RESTModels.Category.RootObject>();
            }
            else
            {
                Debug.WriteLine($"get from service category :{idCategory}");
                category = await Broker.GetCategory(idCategory);

                await _storageService.WriteData(Constants.CATEGORIES + idCategory,  category.SaveAsJson());
                Debug.WriteLine($"saved category :{idCategory}");
            }
            if (category.image != null)
            {
                var c = Categories.Single(o => o.UniqueId == idCategory);
                c.ImageUrl = category.image.standard;
            }
            LoadingCounter--;

        }

        private async Task<Models.UI.Category> GetCategories(Models.UI.Category result)
        {

            var error = string.Empty;
            try
            {
                LoadingCounter++;
                var isCached = await _storageService.Exists(Constants.CATEGORIES, new TimeSpan(10, 0, 0, 0));

                if (isCached)
                {
                    result = JsonConvert.DeserializeObject<Models.UI.Category>(await _storageService.ReadData(Constants.CATEGORIES));
                }
                else
                {
                    result = await Broker.GetCategories();

                    _storageService.Save(Constants.CATEGORIES, result.ToString());
                }

                AppBase.Current.LoadedCategories = result;
                LoadingCounter--;
            }
            catch (Exception ex)
            {
                error = string.Format(International.Translation.ErrorGetting, "categories", ex.Message);

                LoadingCounter--;

            }

            if (!string.IsNullOrEmpty(error))
            {
                await _uxService.ShowAlert(error);
            }

            return result;
        }

        public Home(
          INavigation<NavigationModes> navigationService
          , IStorage storageService
          , ISettings settingsService
          , IUxService uxService
          , IPeerConnector peerConnector)
            : base(navigationService, storageService, settingsService, uxService, peerConnector)
        {

            Categories.Add(new Models.UI.Category { IndexOf = 1, Order = 0, Name = "pc", UniqueId = "pc" });
            Categories.Add(new Models.UI.Category { IndexOf = 1, Order = 1, Name = "electronics", UniqueId = "electronics" });
            Categories.Add(new Models.UI.Category { IndexOf = 1, Order = 2, Name = "media player", UniqueId = "media player" });
            Categories.Add(new Models.UI.Category { IndexOf = 1, Order = 3, Name = "computer hardware", UniqueId = "computer hardware" });
            Categories.Add(new Models.UI.Category { IndexOf = 1, Order = 4, Name = "game console", UniqueId = "game console" });
            Categories.Add(new Models.UI.Category { IndexOf = 1, Order = 5, Name = "car and truck", UniqueId = "car and truck" });
            Categories.Add(new Models.UI.Category { IndexOf = 1, Order = 6, Name = "appliance", UniqueId = "appliance" });
            Categories.Add(new Models.UI.Category { IndexOf = 1, Order = 7, Name = "mac", UniqueId = "mac" });
            Categories.Add(new Models.UI.Category { IndexOf = 1, Order = 8, Name = "apparel", UniqueId = "apparel" });
            Categories.Add(new Models.UI.Category { IndexOf = 1, Order = 9, Name = "phone", UniqueId = "phone" });
            Categories.Add(new Models.UI.Category { IndexOf = 1, Order = 10, Name = "camera", UniqueId = "camera" });
            Categories.Add(new Models.UI.Category { IndexOf = 1, Order = 11, Name = "vehicle", UniqueId = "vehicle" });
            Categories.Add(new Models.UI.Category { IndexOf = 1, Order = 12, Name = "tablet", UniqueId = "tablet" });
            Categories.Add(new Models.UI.Category { IndexOf = 1, Order = 13, Name = "household", UniqueId = "household" });
            Categories.Add(new Models.UI.Category { IndexOf = 1, Order = 14, Name = "skills", UniqueId = "skills" });


        }
    }
}

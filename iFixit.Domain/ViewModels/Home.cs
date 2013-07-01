using GalaSoft.MvvmLight.Command;
using iFixit.Domain.Code;
using iFixit.Domain.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ServicesEngine = iFixit.Domain.Services.V2_0;
using RESTModels = iFixit.Domain.Models.REST.V2_0;

namespace iFixit.Domain.ViewModels
{
    public class Home : BaseViewModel
    {



        #region "Properties






        private ObservableCollection<Models.UI.Category> _Categories = new ObservableCollection<Models.UI.Category>();
        public ObservableCollection<Models.UI.Category> Categories
        {
            get { return this._Categories; }
            set
            {
                if (_Categories != value)
                {
                    _Categories = value;
                    NotifyPropertyChanged();
                }
            }
        }


        private string _MainCollectionImage;
        public string MainCollectionImage
        {
            get { return this._MainCollectionImage; }
            set
            {
                if (_MainCollectionImage != value)
                {
                    _MainCollectionImage = value;
                    NotifyPropertyChanged();
                }
            }
        }


        private string _MainCollectionName;
        public string MainCollectionName
        {
            get { return this._MainCollectionName; }
            set
            {
                if (_MainCollectionName != value)
                {
                    _MainCollectionName = value;
                    NotifyPropertyChanged();
                }
            }
        }


        private string _MainCollectionDescriptions;
        public string MainCollectionDescriptions
        {
            get { return this._MainCollectionDescriptions; }
            set
            {
                if (_MainCollectionDescriptions != value)
                {
                    _MainCollectionDescriptions = value;
                    NotifyPropertyChanged();
                }
            }
        }


        private bool _SelectionMode = false;
        public bool SelectionMode
        {
            get { return this._SelectionMode; }
            set
            {
                if (_SelectionMode != value)
                {
                    _SelectionMode = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private Models.UI.Category _SelectedCategory;
        public Models.UI.Category SelectedCategory
        {
            get { return this._SelectedCategory; }
            set
            {
                if (_SelectedCategory != value)
                {
                    _SelectedCategory = value;
                    NotifyPropertyChanged();
                }
            }
        }


        private Models.UI.HomeItem _SelectedGuide;
        public Models.UI.HomeItem SelectedGuide
        {
            get { return this._SelectedGuide; }
            set
            {
                if (_SelectedGuide != value)
                {
                    _SelectedGuide = value;
                    NotifyPropertyChanged();
                }
            }
        }


        private ObservableCollection<Models.UI.HomeItem> _CollectionsItems = new ObservableCollection<Models.UI.HomeItem>();
        public ObservableCollection<Models.UI.HomeItem> CollectionsItems
        {
            get { return this._CollectionsItems; }
            set
            {
                if (_CollectionsItems != value)
                {
                    _CollectionsItems = value;
                    NotifyPropertyChanged();
                }
            }
        }


        private bool _HasFavorites = false;
        public bool HasFavorites
        {
            get { return this._HasFavorites; }
            set
            {
                if (_HasFavorites != value)
                {
                    _HasFavorites = value;
                    NotifyPropertyChanged();
                }
            }
        }


        private ObservableCollection<Models.UI.HomeItem> _SelectedFavoritesItems = new ObservableCollection<Models.UI.HomeItem>();
        public ObservableCollection<Models.UI.HomeItem> SelectedFavoritesItems
        {
            get { return this._SelectedFavoritesItems; }
            set
            {
                if (_SelectedFavoritesItems != value)
                {
                    _SelectedFavoritesItems = value;

                    NotifyPropertyChanged();
                }
            }
        }

        private ObservableCollection<Models.UI.HomeItem> _FavoritesItems = new ObservableCollection<Models.UI.HomeItem>();
        public ObservableCollection<Models.UI.HomeItem> FavoritesItems
        {
            get { return this._FavoritesItems; }
            set
            {
                if (_FavoritesItems != value)
                {
                    _FavoritesItems = value;
                    if (_FavoritesItems.Count > 0) { HasFavorites = true; } else { HasFavorites = false; }
                    NotifyPropertyChanged();
                }
            }
        }


        private int _NextCollection;
        public int NextCollection
        {
            get { return this._NextCollection; }
            set
            {
                if (_NextCollection != value)
                {
                    _NextCollection = value;
                    NotifyPropertyChanged();
                }
            }
        }


        #endregion

        #region Commands



        private RelayCommand<int> _GoToNextCollection;
        public RelayCommand<int> GoToNextCollection
        {
            get
            {
                return _GoToNextCollection ?? (_GoToNextCollection = new RelayCommand<int>(
                 async (param) =>
                 {
                     LoadingCounter++;
                     CollectionsItems.Clear();
                     var _ResultsCollections = await getCollections(param);
                     Task[] guidesToLoad = new Task[_ResultsCollections[param].guideids.Count];

                     for (int i = 0; i < _ResultsCollections[param].guideids.Count; i++)
                     {
                         guidesToLoad[i] = GetGuide(_ResultsCollections[param].guideids[i].ToString());

                     }

                     await Task.WhenAll(guidesToLoad);

                     LoadingCounter--;
                 }));
            }
        }



        private RelayCommand _DeleteFavorites;
        public RelayCommand DeleteFavorites
        {
            get
            {
                return _DeleteFavorites ?? (_DeleteFavorites = new RelayCommand(
                 async () =>
                 {
                     LoadingCounter++;

                     Task[] toDelete = new Task[SelectedFavoritesItems.Count];
                     Task[] foldersToDelete = new Task[SelectedFavoritesItems.Count];
                     for (int i = 0; i < SelectedFavoritesItems.Count; i++)
                     {
                         toDelete[i] = Broker.RemoveFavorites(AppBase.Current.User, SelectedFavoritesItems[i].UniqueId);
                         foldersToDelete[i] = _storageService.RemoveFolder(string.Format(Constants.GUIDE_CACHE_FOLDER, SelectedFavoritesItems[i].UniqueId));
                     }

                     await Task.WhenAll(toDelete);
                     await Task.WhenAll(foldersToDelete);


                     await GetUserFavorites();

                     this.SelectionMode = !this.SelectionMode;
                     LoadingCounter--;
                 }));
            }
        }

        private RelayCommand _SetSelectionMode;
        public RelayCommand SetSelectionMode
        {
            get
            {
                return _SetSelectionMode ?? (_SetSelectionMode = new RelayCommand(
                async () =>
                {
                    if (this.FavoritesItems.Count == 0)
                    {
                        _navigationService.Navigate<Login>(false);
                    }
                    else
                    {
                        this.SelectionMode = !this.SelectionMode;
                    }


                }));
            }
        }


        private RelayCommand<Models.UI.HomeItem> _TapFavorite;
        public RelayCommand<Models.UI.HomeItem> TapFavorite
        {
            get
            {
                return _TapFavorite ?? (_TapFavorite = new RelayCommand<Models.UI.HomeItem>(
                 (SelectedGuide) =>
                 {

                     try
                     {

                         LoadingCounter++;

                         if (!SelectionMode)
                         {
                             AppBase.Current.GuideId = SelectedGuide.UniqueId;
                             _navigationService.Navigate<Guide>(true, SelectedGuide.UniqueId);
                             SelectedGuide = null;
                             LoadingCounter--;
                         }
                         else
                         {
                             // Edit List

                         }
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



        private RelayCommand _GetFavorites;
        public RelayCommand GetFavorites
        {
            get
            {
                return _GetFavorites ?? (_GetFavorites = new RelayCommand(
                async () =>
                {

                    try
                    {


                        await GetUserFavorites();

                    }
                    catch (Exception ex)
                    {
                        LoadingCounter--;
                        throw ex;
                    }

                }));
            }
        }


        private RelayCommand<Models.UI.HomeItem> _GoToGuide;
        public RelayCommand<Models.UI.HomeItem> GoToGuide
        {
            get
            {
                return _GoToGuide ?? (_GoToGuide = new RelayCommand<iFixit.Domain.Models.UI.HomeItem>(
                 (SelectedGuide) =>
                 {

                     try
                     {
                         LoadingCounter++;
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


        private RelayCommand<Models.UI.Category> _GoToCategory;
        public RelayCommand<Models.UI.Category> GoToCategory
        {
            get
            {
                return _GoToCategory ?? (_GoToCategory = new RelayCommand<Models.UI.Category>(
                 (selectedCategory) =>
                 {

                     try
                     {
                         LoadingCounter++;
                         AppBase.Current.CurrentPage = 0;
                         SelectedCategory = null;
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


        private RelayCommand _LoadHome;
        public RelayCommand LoadHome
        {

            get
            {
                return _LoadHome ?? (_LoadHome = new RelayCommand(async
                    () =>
                {
                    await LoadUser();

                    bool firstLoad = false;

                    switch (this.NavigationType)
                    {
                        case NavigationModes.New:
                            firstLoad = true;

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

                                Models.UI.Category Result = null;
                                Models.REST.V0_1.Collections collections = null;

                                // must do paralel ....
                               // var _ResultsCategory = GetCategories(Result);
                                var _ResultsCollections = getCollections(0);
                                var _ResultLogin = GetUserFavorites();
                                await Task.WhenAll( _ResultsCollections, _ResultLogin);

                                //Result = _ResultsCategory.Result;
                                //TODO: New UI
                                //Categories = Result.Items;
                                collections = _ResultsCollections.Result;


                                int howManyCallsToDo = collections[0].guideids.Count + Categories.Count;
                                Task[] guidesToLoad = new Task[collections[0].guideids.Count];
                                Task[] categoriesToLoad = new Task[Categories.Count];



                                for (int i = 0; i < Categories.Count; i++)
                                {
                                    categoriesToLoad[i] = GetCategoryContent(Categories[i].Name);

                                }

                                for (int i = 0; i < collections[0].guideids.Count; i++)
                                {
                                    guidesToLoad[i] = GetGuide(collections[0].guideids[i].ToString());

                                }

                               await Task.WhenAll(guidesToLoad);
                                await Task.WhenAll(categoriesToLoad);




                            }
                            catch (Exception ex)
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

        private async Task<Models.REST.V0_1.Collections> getCollections(int idx)
        {

            try
            {
                Models.REST.V0_1.Collections collections = new Models.REST.V0_1.Collections();
                LoadingCounter++;
                var isCollectionsCached = await _storageService.Exists(Constants.COLLECTIONS);
                if (isCollectionsCached)
                {
                    var rd = await _storageService.ReadData(Constants.COLLECTIONS);
                    collections = rd.LoadFromJson<Models.REST.V0_1.Collections>();
                }
                else
                {
                    collections = await Broker.GetCollections();
                    await _storageService.WriteData(Constants.COLLECTIONS, await collections.SaveAsJson());

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
                throw ex;
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
                if (_settingsService.IsConnectedToInternet())
                {
                    var r = await Broker.GetFavorites(AppBase.Current.User);
                    await _storageService.WriteData(Constants.FAVORITES, await r.SaveAsJson());
                    BindFavorites(r);
                }
                else
                {

                    await LoadLocalFavories();
                }
            }
            else
            {
                FavoritesItems.Clear();
            }
            LoadingCounter--;
        }

        private void BindFavorites(RESTModels.Favorites.RootObject[] r)
        {
            FavoritesItems.Clear();
            HasFavorites = false;
            if (r != null && r.Count() > 0)
            {
                HasFavorites = true;
                foreach (var item in r)
                {
                    if (!FavoritesItems.Any(o => o.UniqueId == item.guide.guideid.ToString()))
                        FavoritesItems.Add(new Models.UI.HomeItem
                        {
                            Name = item.guide.title,
                            UniqueId = item.guide.guideid.ToString(),
                            ImageUrl = item.guide.image != null ? item.guide.image.standard : ""
                        });
                    //TODO: check if guide is stored and downloaded
                }
            }

        }

        private async Task GetGuide(string idGuide)
        {
            LoadingCounter++;
            string cachedItemName = string.Format(Constants.GUIDE, idGuide);
            Debug.WriteLine(string.Format("going for guide :{0}", idGuide));
            var isCategoryCached = await _storageService.Exists(cachedItemName);

            RESTModels.Guide.RootObject guide = null;
            if (isCategoryCached)
            {
                Debug.WriteLine(string.Format("Cached :{0}", idGuide));
                var rd = await _storageService.ReadData(cachedItemName);
                guide = rd.LoadFromJson<RESTModels.Guide.RootObject>();
            }
            else
            {
                Debug.WriteLine(string.Format("Calling Guide :{0}", idGuide));
                guide = await Broker.GetGuide(idGuide);
                Debug.WriteLine(string.Format("Saving Guide :{0}", idGuide));
                await _storageService.WriteData(cachedItemName, await guide.SaveAsJson());
            }

            CollectionsItems.Add(new Models.UI.HomeItem
            {
                Name = guide.title,
                ImageUrl = guide.image.standard,
                UniqueId = guide.guideid.ToString()
            });
            LoadingCounter--;

        }

        private async Task GetCategoryContent(string idCategory)
        {

            LoadingCounter++;
            Debug.WriteLine(string.Format("going for category :{0}", idCategory));
            var isCategoryCached = await _storageService.Exists(Constants.CATEGORIES + idCategory);
            RESTModels.Category.RootObject category = null;
            if (isCategoryCached)
            {
                var rd = await _storageService.ReadData(Constants.CATEGORIES + idCategory);
                category = rd.LoadFromJson<RESTModels.Category.RootObject>();
            }
            else
            {
                category = await Broker.GetCategory(idCategory);
                await _storageService.WriteData(Constants.CATEGORIES + idCategory, await category.SaveAsJson());
            }
            if (category.image != null)
            {
              var c =    Categories.Single(o => o.UniqueId == idCategory);
              c.ImageUrl = category.image.standard;
            }
            LoadingCounter--;

        }

        private async Task<Models.UI.Category> GetCategories(Models.UI.Category Result)
        {

            string error = string.Empty;
            try
            {
                LoadingCounter++;
                var isCached = await _storageService.Exists(Constants.CATEGORIES);

                if (isCached)
                {
                    Result = JsonConvert.DeserializeObject<Models.UI.Category>(await _storageService.ReadData(Constants.CATEGORIES));
                }
                else
                {
                    Result = await Broker.GetCategories();

                    _storageService.Save(Constants.CATEGORIES, Result.ToString());
                }

                AppBase.Current.LoadedCategories = Result;
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

            return Result;
        }

        public Home(
          INavigation<Domain.Interfaces.NavigationModes> navigationService
          , IStorage storageService
          , ISettings settingsService
          , IUxService uxService
          , IPeerConnector peerConnector)
            : base(navigationService, storageService, settingsService, uxService, peerConnector)
        {

            this.Categories.Add(new Models.UI.Category { Order = 0 , Name = "pc", UniqueId ="pc" });
            this.Categories.Add(new Models.UI.Category { Order = 1, Name = "electronics", UniqueId = "electronics" });
            this.Categories.Add(new Models.UI.Category { Order = 2, Name = "media player", UniqueId = "media player" });
            this.Categories.Add(new Models.UI.Category { Order = 3, Name = "computer hardware", UniqueId = "computer hardware" });
            this.Categories.Add(new Models.UI.Category { Order = 4, Name = "game console", UniqueId = "game console" });
            this.Categories.Add(new Models.UI.Category { Order = 5, Name = "car and truck", UniqueId = "car and truck" });
            this.Categories.Add(new Models.UI.Category { Order = 6, Name = "appliance", UniqueId = "appliance" });
            this.Categories.Add(new Models.UI.Category { Order = 7, Name = "mac", UniqueId = "mac" });
            this.Categories.Add(new Models.UI.Category { Order = 8, Name = "apparel", UniqueId = "apparel" });
            this.Categories.Add(new Models.UI.Category { Order = 9, Name = "phone", UniqueId = "phone" });
            this.Categories.Add(new Models.UI.Category { Order = 10, Name = "camera", UniqueId = "camera" });
            this.Categories.Add(new Models.UI.Category { Order = 11, Name = "vehicle", UniqueId = "vehicle" });
            this.Categories.Add(new Models.UI.Category { Order = 12, Name = "tablet", UniqueId = "tablet" });
            this.Categories.Add(new Models.UI.Category { Order = 13, Name = "household", UniqueId = "household" });
            this.Categories.Add(new Models.UI.Category { Order = 14, Name = "skills", UniqueId = "skills" });

          
        }
    }
}

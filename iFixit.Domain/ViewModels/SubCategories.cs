using GalaSoft.MvvmLight.Command;
using iFixit.Domain.Code;
using iFixit.Domain.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using ServicesEngine = iFixit.Domain.Services.V2_0;
using RESTModels = iFixit.Domain.Models.REST.V2_0;

namespace iFixit.Domain.ViewModels
{
    public class SubCategories : BaseViewModel
    {


        private string _CategoryName = "        ";
        public string CategoryName
        {
            get { return this._CategoryName; }
            set
            {
                if (_CategoryName != value)
                {
                    _CategoryName = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private string _Description;
        public string Description
        {
            get { return this._Description; }
            set
            {
                if (_Description != value)
                {
                    _Description = value;
                    NotifyPropertyChanged();
                }
            }
        }


        private string _LongDescription;
        public string LongDescription
        {
            get { return this._LongDescription; }
            set
            {
                if (_LongDescription != value)
                {
                    _LongDescription = value;
                    NotifyPropertyChanged();
                }
            }
        }


        private string _BackgroundImageUrl;
        public string BackgroundImageUrl
        {
            get { return this._BackgroundImageUrl; }
            set
            {
                if (_BackgroundImageUrl != value)
                {
                    _BackgroundImageUrl = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private string _ImageUrl;
        public string ImageUrl
        {
            get { return this._ImageUrl; }
            set
            {
                if (_ImageUrl != value)
                {
                    _ImageUrl = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private Models.UI.Category _SelectedCategory = null;
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


        private Models.UI.Category _SelectedGroup;
        public Models.UI.Category SelectedGroup
        {
            get { return this._SelectedGroup; }
            set
            {
                if (_SelectedGroup != value)
                {
                    _SelectedGroup = value;
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


        private bool _HasItems;
        public bool HasItems
        {
            get { return _Items.Count > 0; }
            set
            {
                if (_HasItems != value)
                {
                    _HasItems = value;
                    NotifyPropertyChanged();
                }
            }
        }


        private ObservableCollection<Models.UI.Category> _Items = new ObservableCollection<Models.UI.Category>();
        public ObservableCollection<Models.UI.Category> Items
        {
            get { return _Items; }
            set
            {
                if (_Items != value)
                {
                    _Items = value;
                    NotifyPropertyChanged();
                    NotifyPropertyChanged("HasItems");
                }
            }
        }

        private bool _HasGuides = true;
        public bool HasGuides
        {
            get { return _HasGuides; }
            set
            {
                if (_HasGuides != value)
                {
                    _HasGuides = value;
                    NotifyPropertyChanged();
                }
            }
        }


        private ObservableCollection<Models.UI.HomeItem> _Guides = new ObservableCollection<Models.UI.HomeItem>();
        public ObservableCollection<Models.UI.HomeItem> Guides
        {
            get { return this._Guides; }
            set
            {
                if (_Guides != value)
                {
                    _Guides = value;
                    NotifyPropertyChanged();
                    NotifyPropertyChanged("HasGuides");
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


        private string _DisplayTitle;
        public string DisplayTitle
        {
            get { return this._DisplayTitle; }
            set
            {
                if (_DisplayTitle != value)
                {
                    _DisplayTitle = value;
                    NotifyPropertyChanged();
                }
            }
        }


        private RelayCommand<Models.UI.HomeItem> _GoToGuide;
        public RelayCommand<Models.UI.HomeItem> GoToGuide
        {
            get
            {
                return _GoToGuide ?? (_GoToGuide = new RelayCommand<Models.UI.HomeItem>(
                 (item) =>
                 {
                     LoadingCounter++;
                     try
                     {

                         _navigationService.Navigate<Guide>(true, item.UniqueId);

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
                       this.ClearCurrent();
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

        private RelayCommand _UnLoad;
        public RelayCommand UnLoad
        {
            get
            {
                return _UnLoad ?? (_UnLoad = new RelayCommand(
                () =>
                {

                    try
                    {
                        LoadingCounter++;
                        this.ClearCurrent();
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



                     try
                     {
                         if (_settingsService.IsConnectedToInternet())
                         {
                             ClearCurrent();

                             LoadingCounter++;
                             var selectedCategory = this.NavigationParameter<Models.UI.Category>();
                             var selectedItem = await Utils.GetCategoryContent(selectedCategory.UniqueId, _storageService, Broker);
                             CategoryName = selectedItem.wiki_title;
                             DisplayTitle = _uxService.SanetizeHTML(selectedItem.display_title);
                             Description = selectedItem.description;
                             ImageUrl = selectedItem.image.medium;
                             BackgroundImageUrl = selectedItem.image.large;
                             LongDescription = selectedItem.contents_rendered.Replace("&para;", "");


                             this.BreadCrumb = Utils.CreateBreadCrumb(selectedCategory, selectedItem, CategoryName);

                             CategoryBreadcrumb = BreadCrumb.Last().Name;
                             BreadCrumb.Remove(BreadCrumb.Last());


                             foreach (var item in selectedItem.children)
                             {


                                 Items.Add(new Models.UI.Category
                                 {
                                     Name = item.Trim()
                                     ,
                                     UniqueId = item
                                     ,
                                     IndexOf = 1
                                 });
                             }



                             if (AppBase.Current.ExtendeInfoApp)
                             {

                                 try
                                 {
                                     foreach (var item in selectedItem.guides)
                                     {
                                         Guides.Add(new Models.UI.HomeItem
                                         {

                                             Name = item.title.Replace("&quot;", "''")
                                                ,
                                             UniqueId = item.guideid.ToString()
                                                ,
                                             ImageUrl = item.image != null ? item.image.standard : ""
                                                ,
                                             BigImageUrl = item.image != null ? (item.image.large != "" ? item.image.large : item.image.medium) : ""
                                                ,
                                             IndexOf = 1
                                         });

                                         HasGuides = true;
                                     }
                                     if (this.Guides.Count == 0)
                                         HasGuides = false;
                                     else
                                         HasGuides = true;
                                 }
                                 catch (Exception)
                                 {

                                     throw;
                                 }


                                 Task[] categoriesToLoad = new Task[Items.Count];
                                 for (int i = 0; i < Items.Count; i++)
                                 {
                                     categoriesToLoad[i] = GetCategoryContent(Items[i].Name);
                                 }




                                 await Task.WhenAll(categoriesToLoad);


                             } LoadingCounter--;
                         }
                         else
                         {
                             await _uxService.ShowAlert(International.Translation.NoConnection);

                             LoadingCounter++;
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



        private void ClearCurrent()
        {
            Items.Clear();
            Guides.Clear();
            BreadCrumb.Clear();
            this.BackgroundImageUrl = string.Empty;
            this.CategoryName = string.Empty;
            this.Description = string.Empty;
            this.LongDescription = string.Empty;
            this.ImageUrl = string.Empty;
            this.CategoryBreadcrumb = string.Empty;
            this.DisplayTitle = string.Empty;
            this.ImageUrl = string.Empty;
            HasGuides = true;
        }
        private async Task GetCategoryContent(string idCategory)
        {

            LoadingCounter++;
            try
            {
                Debug.WriteLine(string.Format("going for category :{0}", idCategory));
                var isCategoryCached = await _storageService.Exists(Constants.CATEGORIES + idCategory, new TimeSpan(1, 0, 0, 0));
                RESTModels.Category.RootObject category = null;
                if (isCategoryCached)
                {
                    var rd = await _storageService.ReadData(Constants.CATEGORIES + idCategory);
                    category = rd.LoadFromJson<RESTModels.Category.RootObject>();
                }
                else
                {
                    category = await Broker.GetCategory(idCategory);
                    await _storageService.WriteData(Constants.CATEGORIES + idCategory,  category.SaveAsJson());
                }
                if (category.image != null)
                {
                    var c = Items.SingleOrDefault(o => o.UniqueId == idCategory);
                    if (c != null)
                        c.ImageUrl = category.image.standard;
                }
            }
            catch (Exception)
            {

                throw;
            }
            LoadingCounter--;

        }


        public SubCategories(INavigation<Domain.Interfaces.NavigationModes> navigationService, IStorage storageService, ISettings settingsService, IUxService uxService, IPeerConnector peerConnector)
            : base(navigationService, storageService, settingsService, uxService, peerConnector)
        {


        }
    }
}

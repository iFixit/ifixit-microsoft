using GalaSoft.MvvmLight.Command;
using iFixit.Domain.Code;
using iFixit.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iFixit.Domain.ViewModels
{
    public class SubCategories : BaseViewModel
    {


        private string _CategoryName;
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


        private ObservableCollection<Models.UI.Category> _Items = new ObservableCollection<Models.UI.Category>();
        public ObservableCollection<Models.UI.Category> Items
        {
            get { return this._Items; }
            set
            {
                if (_Items != value)
                {
                    _Items = value;
                    NotifyPropertyChanged();
                }
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
                       var x = await Utils.GetCategoryContent(selectedCategory.Name, _storageService, Broker);
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



                     try
                     {
                         Items.Clear();

                         LoadingCounter++;
                         var xy = this.NavigationParameter<Models.UI.Category>();
                         CategoryName = xy.Name;
                         var x = await Utils.GetCategoryContent(xy.Name, _storageService, Broker);
                         Description = x.description;
                         ImageUrl = x.image.medium;
                         BackgroundImageUrl = x.image.large;


                         foreach (var item in x.children)
                         {


                             Items.Add(new Models.UI.Category
                             {
                                 Name = item.ToLower(),
                                 UniqueId = item.ToLower()

                             });
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




        public SubCategories(INavigation<Domain.Interfaces.NavigationModes> navigationService, IStorage storageService, ISettings settingsService, IUxService uxService, IPeerConnector peerConnector)
            : base(navigationService, storageService, settingsService, uxService, peerConnector)
        {


        }
    }
}

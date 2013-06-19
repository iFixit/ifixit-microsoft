using GalaSoft.MvvmLight.Command;
using iFixit.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iFixit.Domain.ViewModels
{
    public class About : BaseViewModel
    {

        private int _SelectedIndex=0;
        public int SelectedIndex
        {
            get { return this._SelectedIndex; }
            set
            {
                if (_SelectedIndex != value)
                {
                    _SelectedIndex = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private RelayCommand _Load;
        public RelayCommand Load
        {
            get
            {
                return _Load ?? (_Load = new RelayCommand(
                  () =>
                  {

                      try
                      {

                          SelectedIndex = this.NavigationParameter<int>();


                      }
                      catch (Exception ex)
                      {
                          LoadingCounter--;
                          throw ex;
                      }

                  }));
            }

        }

        public About(INavigation<Domain.Interfaces.NavigationModes> navigationService, IStorage storageService, ISettings settingsService, IUxService uxService, IPeerConnector peerConnector)
            : base(navigationService, storageService, settingsService, uxService, peerConnector)
        {


            this.PageTitle = International.Translation.About;
        }
    }
}

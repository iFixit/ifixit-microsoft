using GalaSoft.MvvmLight.Command;
using iFixit.Domain.Code;
using iFixit.Domain.Interfaces;
using System;

namespace iFixit.Domain.ViewModels
{
    public class Profile : BaseViewModel
    {



        private Models.UI.User _user;
        public Models.UI.User User
        {
            get { return this._user; }
            set
            {
                if (_user != value)
                {
                    _user = value;
                    NotifyPropertyChanged();
                }
            }
        }
        
        

        private RelayCommand _loadProfile;
        public RelayCommand LoadProfile
        {
            get
            {
                return _loadProfile ?? (_loadProfile = new RelayCommand(
                  () =>
                  {

                      try
                      {

                          this.User = AppBase.Current.User;


                      }
                      catch (Exception )
                      {
                          LoadingCounter--;
                          throw ;
                      }

                  }));
            }

        }

        private RelayCommand _doLogout;
        public RelayCommand DoLogout
        {
            get
            {
                return _doLogout ?? (_doLogout = new RelayCommand(
                 async () =>
                 {

                     try
                     {
                         LoadingCounter++;
                         await Utils.DoLogOut(_storageService, _uxService, Broker);
                         _uxService.DoLogOff();
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

     




           public Profile(INavigation<Domain.Interfaces.NavigationModes> navigationService, IStorage storageService, ISettings settingsService, IUxService uxService, IPeerConnector peerConnector)
            : base(navigationService, storageService, settingsService, uxService, peerConnector)
        {


          
        }

    }
}

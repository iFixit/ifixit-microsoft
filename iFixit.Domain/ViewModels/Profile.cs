using GalaSoft.MvvmLight.Command;
using iFixit.Domain.Code;
using iFixit.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iFixit.Domain.ViewModels
{
    public class Profile : BaseViewModel
    {



        private Models.UI.User _User;
        public Models.UI.User User
        {
            get { return this._User; }
            set
            {
                if (_User != value)
                {
                    _User = value;
                    NotifyPropertyChanged();
                }
            }
        }
        
        

        private RelayCommand _LoadProfile;
        public RelayCommand LoadProfile
        {
            get
            {
                return _LoadProfile ?? (_LoadProfile = new RelayCommand(
                  () =>
                  {

                      try
                      {

                          this.User = AppBase.Current.User;


                      }
                      catch (Exception ex)
                      {
                          LoadingCounter--;
                          throw ex;
                      }

                  }));
            }

        }

        private RelayCommand _DoLogout;
        public RelayCommand DoLogout
        {
            get
            {
                return _DoLogout ?? (_DoLogout = new RelayCommand(
                 async () =>
                 {

                     try
                     {
                         LoadingCounter++;
                         await Utils.DoLogOut(_storageService, _uxService, Broker);
                         _uxService.DoLogOff();
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

     




           public Profile(INavigation<Domain.Interfaces.NavigationModes> navigationService, IStorage storageService, ISettings settingsService, IUxService uxService, IPeerConnector peerConnector)
            : base(navigationService, storageService, settingsService, uxService, peerConnector)
        {


          
        }

    }
}

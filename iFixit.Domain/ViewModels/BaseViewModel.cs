using GalaSoft.MvvmLight.Command;
using iFixit.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iFixit.Domain.Code;
using Logger = iFixit.UI.Services.Logger;
using ServicesEngine = iFixit.Domain.Services.V2_0;
using RESTModels = iFixit.Domain.Models.REST.V2_0;

namespace iFixit.Domain.ViewModels
{
    public partial class BaseViewModel : Models.ModelBase //  Models.ModelBase
    {

        public readonly INavigation<Domain.Interfaces.NavigationModes> _navigationService;
        public readonly IStorage _storageService;
        public readonly ISettings _settingsService;
        public readonly IUxService _uxService;
        public readonly IPeerConnector _peerConnector;

        public ServicesEngine.ServiceBroker Broker;

        #region Properties

        public string navigationParameterJson;

        public T NavigationParameter<T>()
        {
            return navigationParameterJson.LoadFromJson<T>();
        }


        public Interfaces.NavigationModes NavigationType { get; set; }

        private bool _IsOffline = false;
        public bool IsOffline
        {
            get { return !_settingsService.IsConnectedToInternet(); }

        }


        private int loadingCounter = 0;
        public int LoadingCounter
        {
            get { return loadingCounter; }
            set
            {
                loadingCounter = value;
                if (value != loadingCounter)
                {
                    loadingCounter = value;
                    // NotifyPropertyChanged();
                }
                if (loadingCounter < 0)
                    loadingCounter = 0;

                if (loadingCounter > 0)
                {
                    IsLoading = true;
                }
                else
                {
                    IsLoading = false;
                }
            }
        }

        private bool isLoading = false;
        public bool IsLoading
        {
            get { return isLoading; }
            set
            {
                if (value != isLoading)
                {
                    isLoading = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private string pageTitle = string.Empty;
        public string PageTitle
        {
            get { return pageTitle; }
            set
            {
                if (value != pageTitle)
                {
                    pageTitle = value;
                    NotifyPropertyChanged();
                }
            }
        }


        private string appName = "IFIXIT";
        public string ApplicationName
        {
            get { return appName; }
            set
            {
                if (value != appName)
                {
                    appName = value;
                    NotifyPropertyChanged();
                }
            }
        }


        private bool _IsAuthenticated = false;
        public bool IsAuthenticated
        {
            get { return this._IsAuthenticated; }
            set
            {
                if (_IsAuthenticated != value)
                {
                    _IsAuthenticated = value;
                    NotifyPropertyChanged();
                }
            }
        }



        #endregion

        private RelayCommand<int> _GoToAbout;
        public RelayCommand<int> GoToAbout
        {
            get
            {
                return _GoToAbout ?? (_GoToAbout = new RelayCommand<int>(
                 (idx) =>
                 {
                     LoadingCounter++;
                     try
                     {

                         _navigationService.Navigate<About>(false, idx);
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

        private RelayCommand _GoHome;
        public RelayCommand GoHome
        {
            get
            {
                return _GoHome ?? (_GoHome = new RelayCommand(
                  () =>
                  {

                      try
                      {

                          _navigationService.Navigate<Home>(false);

                      }
                      catch (Exception ex)
                      {
                          LoadingCounter--;
                          throw ex;
                      }

                  }));
            }

        }

        private RelayCommand _OpenSpeech;
        public RelayCommand OpenSpeech
        {
            get
            {
                return _OpenSpeech ?? (_OpenSpeech = new RelayCommand(
                 async () =>
                 {

                     try
                     {

                         await _uxService.OpenSpeechUI();

                     }
                     catch (Exception ex)
                     {
                         LoadingCounter--;
                         throw ex;
                     }

                 }));
            }

        }

        private RelayCommand _GoToSearch;
        public RelayCommand GoToSearch
        {
            get
            {
                return _GoToSearch ?? (_GoToSearch = new RelayCommand(
                 () =>
                 {
                     LoadingCounter++;
                     try
                     {

                      
                         _uxService.OpenSearch();
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

        private RelayCommand _DoLogin;
        public RelayCommand DoLogin
        {
            get
            {
                return _DoLogin ?? (_DoLogin = new RelayCommand(
                  () =>
                  {

                      try
                      {

                          if (AppBase.Current.User == null)
                          {
                              _navigationService.Navigate<Login>(false);
                          }
                          else
                          {
                              _navigationService.Navigate<Profile>(false);
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



        public void BindAuthentication(Models.REST.V2_0.Login.Output.RootObject x)
        {
            AppBase.Current.User = new iFixit.Domain.Models.UI.User
            {
                Token = x.authToken,
                UserId = x.userid,
                UserName = x.username,
                Email = x.email,
                ImageUrl = x.image.thumbnail

            };
            IsAuthenticated = true;
        }


        public BaseViewModel()
        {

        }


        public BaseViewModel(
            INavigation<Domain.Interfaces.NavigationModes> navigationService
            , IStorage storageService
            , ISettings settingsService
            , IUxService uxService
            , IPeerConnector peerConnector)
        {

            _navigationService = navigationService;
            _storageService = storageService;
            _settingsService = settingsService;
            _uxService = uxService;
            _peerConnector = peerConnector;
            Broker = new ServicesEngine.ServiceBroker(_settingsService.AppKey());
            _peerConnector.ConnectionStatusChanged += _peerConnector_ConnectionStatusChanged;
            _peerConnector.GuideReceived += _peerConnector_GuideReceived;
        }

        public bool IsConnected { get; private set; }
        public event EventHandler<ConnectionStatusChangedEventArgs> ConnectionStatusChanged;
        public event EventHandler<GuideReceivedEventArgs> GuideReceived;

        void _peerConnector_GuideReceived(object sender, GuideReceivedEventArgs e)
        {
            throw new NotImplementedException();
        }

        void _peerConnector_ConnectionStatusChanged(object sender, ConnectionStatusChangedEventArgs e)
        {
            switch (e.Status)
            {
                case ConnectionStatus.Completed:
                    IsConnected = true;
                    break;
                case ConnectionStatus.Canceled:
                    // If I am already connected, the canel just means we accidentally tapped again and I want to stay connected.
                    // If I am not already connected, IsConnected is already false, so no need to do anything.
                    break;
                case ConnectionStatus.Disconnected:
                case ConnectionStatus.Failed:
                case ConnectionStatus.TapNotSupported:
                    IsConnected = false;
                    break;
            }

            if (ConnectionStatusChanged != null)
                ConnectionStatusChanged(this, new ConnectionStatusChangedEventArgs() { Status = e.Status });
        }



    }
}

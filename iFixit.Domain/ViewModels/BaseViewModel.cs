using GalaSoft.MvvmLight.Command;
using iFixit.Domain.Interfaces;
using System;
using iFixit.Domain.Code;
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
                if (value == isLoading) return;
                isLoading = value;
                NotifyPropertyChanged();
            }
        }

        private string pageTitle = string.Empty;
        public string PageTitle
        {
            get { return pageTitle; }
            set
            {
                if (value == pageTitle) return;
                pageTitle = value;
                NotifyPropertyChanged();
            }
        }


        private string appName = "IFIXIT";
        public string ApplicationName
        {
            get { return appName; }
            set
            {
                if (value == appName) return;
                appName = value;
                NotifyPropertyChanged();
            }
        }


        private bool _CanGoBack=false;
        public bool CanGoBack
        {
            get { return _CanGoBack; }
            set
            {
                _CanGoBack = value;
                NotifyPropertyChanged();
            }
        }


        private bool _isAuthenticated = false;
        public bool IsAuthenticated
        {
            get { return this._isAuthenticated; }
            set
            {
                if (_isAuthenticated == value) return;
                _isAuthenticated = value;
                NotifyPropertyChanged();
            }
        }



        #endregion

        private RelayCommand<int> _goToAbout;
        public RelayCommand<int> GoToAbout
        {
            get
            {
                return _goToAbout ?? (_goToAbout = new RelayCommand<int>(
                 (idx) =>
                 {
                     LoadingCounter++;
                     try
                     {

                         _navigationService.Navigate<About>(false, idx);
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

        private RelayCommand _goHome;
        public RelayCommand GoHome
        {
            get
            {
                return _goHome ?? (_goHome = new RelayCommand(
                  () =>
                  {

                      try
                      {
                          LoadingCounter++;
                          _navigationService.Navigate<Home>(false, "-1");
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

        private RelayCommand _goSearchResults;
        public RelayCommand GoSearchResults
        {
            get
            {
                return _goSearchResults ?? (_goSearchResults = new RelayCommand(
                  () =>
                  {

                      try
                      {
                          LoadingCounter++;
                          _navigationService.Navigate<Search>(false);
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

        private RelayCommand _goBack;
        public RelayCommand GoBack
        {
            get
            {
                return _goBack ?? (_goBack = new RelayCommand(
                  () =>
                  {

                      try
                      {

                          _navigationService.GoBack();

                      }
                      catch (Exception )
                      {
                          LoadingCounter--;
                          //throw ex;
                      }

                  }));
            }

        }

        private RelayCommand _openSpeech;
        public RelayCommand OpenSpeech
        {
            get
            {
                return _openSpeech ?? (_openSpeech = new RelayCommand(
                 async () =>
                 {

                     try
                     {

                         await _uxService.OpenSpeechUI();

                     }
                     catch (Exception )
                     {
                         LoadingCounter--;
                         throw ;
                     }

                 }));
            }

        }

        private RelayCommand _goToSearch;
        public RelayCommand GoToSearch
        {
            get
            {
                return _goToSearch ?? (_goToSearch = new RelayCommand(
                 () =>
                 {
                     LoadingCounter++;
                     try
                     {


                         _uxService.OpenSearch();
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

        private RelayCommand _doLogin;
        public RelayCommand DoLogin
        {
            get
            {
                return _doLogin ?? (_doLogin = new RelayCommand(
                  () =>
                  {

                      try
                      {

                          if (AppBase.Current.User == null)
                          {
                              _uxService.GoToLogin();

                          }
                          else
                          {
                              _uxService.GoToProfile();

                          }


                      }
                      catch (Exception )
                      {
                          LoadingCounter--;
                          throw ;
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

            Broker = new ServicesEngine.ServiceBroker(_settingsService.AppKey(), _settingsService.AppVersion());
            _peerConnector.ConnectionStatusChanged += _peerConnector_ConnectionStatusChanged;
            _peerConnector.GuideReceived += _peerConnector_GuideReceived;
        }

        public bool IsConnected { get; private set; }
        public event EventHandler<ConnectionStatusChangedEventArgs> ConnectionStatusChanged;
        public event EventHandler<GuideReceivedEventArgs> GuideReceived;

        static void _peerConnector_GuideReceived(object sender, GuideReceivedEventArgs e)
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

            ConnectionStatusChanged?.Invoke(this, new ConnectionStatusChangedEventArgs() { Status = e.Status });
        }



    }
}

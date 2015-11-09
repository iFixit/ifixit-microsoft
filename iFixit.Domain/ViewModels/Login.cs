using GalaSoft.MvvmLight.Command;
using iFixit.Domain.Interfaces;
using System;
using iFixit.Domain.Code;

namespace iFixit.Domain.ViewModels
{
    public class Login : BaseViewModel
    {

        #region "properties"

        private string _buttonLabel = International.Translation.Login;
        public string ButtonLabel
        {
            get { return _buttonLabel; }
            set
            {
                if (_buttonLabel == value) return;
                _buttonLabel = value;
                NotifyPropertyChanged();
            }
        }


        private string _email;
        public string Email
        {
            get { return _email; }
            set
            {
                if (_email == value) return;
                _email = value;
                NotifyPropertyChanged();
            }
        }


        private string _password;
        public string Password
        {
            get { return _password; }
            set
            {
                if (_password == value) return;
                _password = value;
                NotifyPropertyChanged();
            }
        }


        private string _confirmationPassword;
        public string ConfirmationPassword
        {
            get { return _confirmationPassword; }
            set
            {
                if (_confirmationPassword == value) return;
                _confirmationPassword = value;

                NotifyPropertyChanged();
            }
        }


         bool _registrationProcess;
        public bool RegistrationProcess
        {
            get { return _registrationProcess; }
            set
            {
                if (_registrationProcess == value) return;
                _registrationProcess = value;

                NotifyPropertyChanged();
            }
        }


        string _userName;
        public string UserName
        {
            get { return _userName; }
            set
            {
                if (_userName == value) return;
                _userName = value;
                NotifyPropertyChanged();
            }
        }


        int _menuIndex ;
        public int MenuIndex
        {
            get { return _menuIndex; }
            set
            {
                if (_menuIndex == value) return;
                _menuIndex = value;
                NotifyPropertyChanged();
            }
        }


        bool _doLoginProcess = true;
        public bool DoLoginProcess
        {
            get { return _doLoginProcess; }
            set
            {
                if (_doLoginProcess == value) return;
                _doLoginProcess = value;
                NotifyPropertyChanged();
            }
        }


         bool _recoverPasswordProcess ;
        public bool RecoverPasswordProcess
        {
            get { return _recoverPasswordProcess; }
            set
            {
                if (_recoverPasswordProcess == value) return;
                _recoverPasswordProcess = value;

                NotifyPropertyChanged();
            }
        }



         string _serviceMessages;
        public string ServiceMessages
        {
            get { return _serviceMessages; }
            set
            {
                if (_serviceMessages == value) return;
                _serviceMessages = value;
                NotifyPropertyChanged();
            }
        }


        #endregion


        private RelayCommand _doLoginP;
        public RelayCommand DoLoginP
        {
            get
            {
                return _doLoginP ?? (_doLoginP = new RelayCommand(
                  async () =>
                  {
                      var hasError = string.Empty;
                      ServiceMessages = string.Empty;
                      try
                      {
                          if (DoLoginProcess)
                          {
                              if (!string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(Password))
                              {
                                  LoadingCounter++;

                                  var user = await Broker.DoLogin(Email, Password);
                                  if (user?.authToken != null)
                                  {
                                      _storageService.Save(Constants.AUTHORIZATION,  user.SaveAsJson());
                                      BindAuthentication(user);

                                      _uxService.DoLogin();

                                      await _uxService.ShowToast(International.Translation.LoginSuccessfull);
                                      LoadingCounter--;
                                      //TODO: How to fire the Add Favorite if login was asked because of this. 
                                  }
                                  else
                                  {
                                      ServiceMessages = International.Translation.ErrorLogin;
                                      await _uxService.LoginMessaging(ServiceMessages);
                                      LoadingCounter--;
                                  }
                              }
                              else
                              {
                                  ServiceMessages = International.Translation.FillForm;
                                  await _uxService.LoginMessaging(International.Translation.FillForm);
                                  LoadingCounter--;
                              }

                          }
                          else if (RegistrationProcess)
                          {

                              try
                              {
                                  if (Password == ConfirmationPassword && !string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(UserName))
                                  {
                                      LoadingCounter++;
                                      var result = await Broker.RegistrationLogin(Email, UserName, Password);

                                      RegistrationProcess = false;
                                      DoLoginProcess = true;
                                      BindAuthentication(result);
                                      LoadingCounter--;
                                      _uxService.DoLogin();


                                  }
                                  else
                                  {
                                      if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(ConfirmationPassword))
                                      {
                                          await _uxService.ShowAlert(International.Translation.FillForm);
                                          LoadingCounter--;
                                      }
                                      else if (Password != ConfirmationPassword)
                                      {
                                          await _uxService.ShowAlert(International.Translation.SamePassword);
                                          LoadingCounter--;
                                      }
                                  }

                              }
                              catch (Exception ex)
                              {
                                  hasError = ex.Message;
                                  LoadingCounter--;

                              }
                              if (!string.IsNullOrEmpty(hasError))
                              {
                                  ServiceMessages = string.Format("{0}:{1}", International.Translation.ErrorRegistration, hasError);
                                  await _uxService.LoginMessaging(ServiceMessages);
                              }
                              LoadingCounter--;
                          }
                          else if (RecoverPasswordProcess)
                          {
                              try
                              {
                                  LoadingCounter++;
                                  LoadingCounter--;
                              }
                              catch (Exception)
                              {
                                  LoadingCounter--;
                                  throw;
                              }
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

        private RelayCommand _doCancel;
        public RelayCommand DoCancel
        {
            get
            {
                return _doCancel ?? (_doCancel = new RelayCommand(
                   () =>
                   {

                       try
                       {

                           RegistrationProcess = false;
                           RecoverPasswordProcess = false;
                           DoLoginProcess = true;
                           Email = Password = ConfirmationPassword = UserName = string.Empty;

                           ButtonLabel = International.Translation.Login;


                       }
                       catch (Exception )
                       {

                           throw ;
                       }

                   }));
            }

        }


        private RelayCommand _startRegistration;
        public RelayCommand StartRegistration
        {
            get
            {
                return _startRegistration ?? (_startRegistration = new RelayCommand(
                   () =>
                   {

                       try
                       {

                           RegistrationProcess = true;
                           ButtonLabel = International.Translation.Register;
                           RecoverPasswordProcess = false;
                           DoLoginProcess = false;

                       }
                       catch (Exception )
                       {
                           LoadingCounter--;
                           throw ;
                       }

                   }));
            }

        }

        private RelayCommand _startRecoverPassword;
        public RelayCommand StartRecoverPassword
        {
            get
            {
                return _startRecoverPassword ?? (_startRecoverPassword = new RelayCommand(
                   () =>
                   {

                       try
                       {

                           RecoverPasswordProcess = true;
                           ButtonLabel = International.Translation.Send;
                           RegistrationProcess = false;
                           DoLoginProcess = false;

                       }
                       catch (Exception )
                       {
                           LoadingCounter--;
                           throw ;
                       }

                   }));
            }

        }


        public Login(INavigation<NavigationModes> navigationService, IStorage storageService, ISettings settingsService, IUxService uxService, IPeerConnector peerConnector)
            : base(navigationService, storageService, settingsService, uxService, peerConnector)
        {


            PageTitle = International.Translation.Login;
        }
    }
}

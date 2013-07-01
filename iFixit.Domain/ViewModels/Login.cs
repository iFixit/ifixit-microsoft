using GalaSoft.MvvmLight.Command;
using iFixit.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iFixit.Domain.Code;

namespace iFixit.Domain.ViewModels
{
    public class Login : BaseViewModel
    {

        #region "properties"

        private string _ButtonLabel = International.Translation.Login;
        public string ButtonLabel
        {
            get { return this._ButtonLabel; }
            set
            {
                if (_ButtonLabel != value)
                {
                    _ButtonLabel = value;
                    NotifyPropertyChanged();
                }
            }
        }


        private string _Email;
        public string Email
        {
            get { return this._Email; }
            set
            {
                if (_Email != value)
                {
                    _Email = value;
                    NotifyPropertyChanged();
                }
            }
        }


        private string _Password;
        public string Password
        {
            get { return this._Password; }
            set
            {
                if (_Password != value)
                {
                    _Password = value;
                    NotifyPropertyChanged();
                }
            }
        }


        private string _ConfirmationPassword;
        public string ConfirmationPassword
        {
            get { return this._ConfirmationPassword; }
            set
            {
                if (_ConfirmationPassword != value)
                {
                    _ConfirmationPassword = value;

                    NotifyPropertyChanged();
                }
            }
        }


        private bool _RegistrationProcess = false;
        public bool RegistrationProcess
        {
            get { return this._RegistrationProcess; }
            set
            {
                if (_RegistrationProcess != value)
                {
                    _RegistrationProcess = value;

                    NotifyPropertyChanged();
                }
            }
        }


        private string _UserName;
        public string UserName
        {
            get { return this._UserName; }
            set
            {
                if (_UserName != value)
                {
                    _UserName = value;
                    NotifyPropertyChanged();
                }
            }
        }


        private int _MenuIndex = 0;
        public int MenuIndex
        {
            get { return this._MenuIndex; }
            set
            {
                if (_MenuIndex != value)
                {
                    _MenuIndex = value;
                    NotifyPropertyChanged();
                }
            }
        }


        private bool _DoLoginProcess = true;
        public bool DoLoginProcess
        {
            get { return this._DoLoginProcess; }
            set
            {
                if (_DoLoginProcess != value)
                {
                    _DoLoginProcess = value;
                    NotifyPropertyChanged();
                }
            }
        }


        private bool _RecoverPasswordProcess = false;
        public bool RecoverPasswordProcess
        {
            get { return this._RecoverPasswordProcess; }
            set
            {
                if (_RecoverPasswordProcess != value)
                {
                    _RecoverPasswordProcess = value;

                    NotifyPropertyChanged();
                }
            }
        }

        #endregion






        private RelayCommand _DoLoginP;
        public RelayCommand DoLoginP
        {
            get
            {
                return _DoLoginP ?? (_DoLoginP = new RelayCommand(
                  async () =>
                  {
                      string hasError = string.Empty;
                      try
                      {
                          if (this.DoLoginProcess)
                          {
                              LoadingCounter++;

                              var user = await Broker.DoLogin(this.Email, this.Password);
                              if (user != null && user.authToken != null)
                              {
                                  _storageService.Save("Authorization", await user.SaveAsJson());
                                  BindAuthentication(user);
                                  _navigationService.GoBack();
                                  LoadingCounter--;
                                  //TODO: How to fire the Add Favorite if login was asked because of this. 
                              }
                              else
                              {

                                  await _uxService.ShowAlert(International.Translation.ErrorLogin);
                                  LoadingCounter--;
                              }
                          }
                          else if (RegistrationProcess)
                          {

                              try
                              {
                                  if (this.Password == this.ConfirmationPassword && !string.IsNullOrEmpty(this.Email) && !string.IsNullOrEmpty(this.UserName))
                                  {
                                      LoadingCounter++;
                                      var result = await Broker.RegistrationLogin(this.Email, this.UserName, this.Password);
                                   
                                      this.RegistrationProcess = false;
                                      this.DoLoginProcess = true;
                                      BindAuthentication(result);
                                      LoadingCounter--;
                                      _navigationService.GoBack();


                                  }
                                  else
                                  {
                                      if (string.IsNullOrEmpty(this.Email) || string.IsNullOrEmpty(this.UserName) || string.IsNullOrEmpty(this.Password) || string.IsNullOrEmpty(this.ConfirmationPassword))
                                      {
                                          await _uxService.ShowAlert(International.Translation.FillForm);
                                          LoadingCounter--;
                                      }
                                      else if (this.Password != this.ConfirmationPassword)
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
                                  await _uxService.ShowAlert(string.Format("{0}:{1}", International.Translation.ErrorRegistration, hasError));

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
                      catch (Exception ex)
                      {
                          LoadingCounter--;
                          throw ex;
                      }

                  }));
            }

        }

        private RelayCommand _DoCancel;
        public RelayCommand DoCancel
        {
            get
            {
                return _DoCancel ?? (_DoCancel = new RelayCommand(
                   () =>
                   {

                       try
                       {

                           RegistrationProcess = false;
                           RecoverPasswordProcess = false;
                           DoLoginProcess = true;
                           ButtonLabel = International.Translation.Login;


                       }
                       catch (Exception ex)
                       {

                           throw ex;
                       }

                   }));
            }

        }


        private RelayCommand _StartRegistration;
        public RelayCommand StartRegistration
        {
            get
            {
                return _StartRegistration ?? (_StartRegistration = new RelayCommand(
                   () =>
                   {

                       try
                       {

                           RegistrationProcess = true;
                           ButtonLabel = International.Translation.Register;
                           RecoverPasswordProcess = false;
                           DoLoginProcess = false;

                       }
                       catch (Exception ex)
                       {
                           LoadingCounter--;
                           throw ex;
                       }

                   }));
            }

        }

        private RelayCommand _StartRecoverPassword;
        public RelayCommand StartRecoverPassword
        {
            get
            {
                return _StartRecoverPassword ?? (_StartRecoverPassword = new RelayCommand(
                   () =>
                   {

                       try
                       {

                           RecoverPasswordProcess = true;
                           ButtonLabel = International.Translation.Send;
                           RegistrationProcess = false;
                           DoLoginProcess = false;

                       }
                       catch (Exception ex)
                       {
                           LoadingCounter--;
                           throw ex;
                       }

                   }));
            }

        }




        public Login(INavigation<Domain.Interfaces.NavigationModes> navigationService, IStorage storageService, ISettings settingsService, IUxService uxService, IPeerConnector peerConnector)
            : base(navigationService, storageService, settingsService, uxService, peerConnector)
        {


            this.PageTitle = International.Translation.Login;
        }
    }
}

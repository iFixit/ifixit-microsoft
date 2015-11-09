using System;
using Windows.ApplicationModel;
using Windows.Networking.Connectivity;

namespace iFixit.W8.UI.Services
{
  public  class UiSettings:Domain.Interfaces.ISettings
    {
        public string AppName()
        {
            throw new NotImplementedException();
        }

        public string AppVersion()
        {
            var version = Package.Current.Id.Version;
            return $"{version.Major}.{version.Minor}";
        }

        public string AppKey()
        {
            return "6f8cb5333f1147170595c875d8bac50a";
        }

        public string PublisherEmail()
        {
            throw new NotImplementedException();
        }

        public string SupportEmail()
        {
            throw new NotImplementedException();
        }

        public string ServiceUrl()
        {
            throw new NotImplementedException();
        }

        public bool IsConnectedToInternet()
        {
            NetworkInformation.NetworkStatusChanged += NetworkInformation_NetworkStatusChanged;

            bool connected = false;

            ConnectionProfile cp = NetworkInformation.GetInternetConnectionProfile();

            if (cp != null)
            {
                NetworkConnectivityLevel cl = cp.GetNetworkConnectivityLevel();

                connected = cl == NetworkConnectivityLevel.InternetAccess;
            }

            return connected;
        }
        void NetworkInformation_NetworkStatusChanged(object sender)
        {



        }
    }
}

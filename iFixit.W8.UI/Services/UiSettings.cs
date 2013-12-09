using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Networking.Connectivity;

namespace iFixit.W8.UI.Services
{
  public  class UiSettings:iFixit.Domain.Interfaces.ISettings
    {
        public string AppName()
        {
            throw new NotImplementedException();
        }

        public string AppVersion()
        {
            PackageVersion version = Package.Current.Id.Version;
            return string.Format("{0}.{1}", version.Major, version.Minor);
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

using Cimbalino.Phone.Toolkit.Helpers;
using Cimbalino.Phone.Toolkit.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace iFixit.UI.Services
{
    public class UiSettings : Domain.Interfaces.ISettings
    {
        public string AppName()
        {
            throw new NotImplementedException();
        }

        public string AppVersion()
        {

            ApplicationManifestService manifest = new ApplicationManifestService();


            return manifest.GetApplicationManifest().App.Version;
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
            return Microsoft.Phone.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
        }
    }
}

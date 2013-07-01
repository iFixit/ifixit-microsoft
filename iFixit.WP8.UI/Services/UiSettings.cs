using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace iFixit.UI.Services
{
    public class UiSettings : Domain.Interfaces.ISettings
    {
        public string AppName()
        {
            throw new NotImplementedException();
        }

        public string AppKey()
        {
            // ReadFromFile
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

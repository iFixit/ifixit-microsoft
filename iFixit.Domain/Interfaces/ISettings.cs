using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iFixit.Domain.Interfaces
{
    public interface ISettings
    {
        string AppName();
        string PublisherEmail();
        string SupportEmail();
        string ServiceUrl();
        bool IsConnectedToInternet();
    }
}

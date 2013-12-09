using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iFixit.Domain.Interfaces
{
    public interface ISettings
    {
        string AppName();
        string AppVersion();
        string AppKey();
        string PublisherEmail();
        string SupportEmail();
        string ServiceUrl();
        bool IsConnectedToInternet();
    }
}

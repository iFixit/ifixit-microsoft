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

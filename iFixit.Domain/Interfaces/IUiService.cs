using System.Threading.Tasks;


namespace iFixit.Domain.Interfaces
{
   public interface IUxService
    {
       string SanetizeHTML(string html);

        Task ShowAlert(string m);

        Task ShowToast(string m);

        string PrepHTML(string content, string backgroundColor, string fontColor);

        string CleanHTML(string content);

        string BackgroundColor();

        string FontColor();

        void OpenIe(string url);

        void Share(string url, string title);

        void OpenSearch();

        void CancelSpeech();

        Task OpenSpeechUI();

        Task OpenTextSpeechUI(ViewModels.Guide pagesToRead);

        Task OpenBrowser(string url);

        void ShowVideo(string url);

        void GoToLogin();

        void GoToProfile();

        void DoLogin();

        void DoLogOff();

        Task LoginMessaging(string Message);
    }
}

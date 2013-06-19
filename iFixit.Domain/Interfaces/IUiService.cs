using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace iFixit.Domain.Interfaces
{
   public interface IUxService
    {
        Task ShowAlert(string m);

        Task ShowToast(string m);

        string PrepHTML(string content, string BackgroundColor, string FontColor);

        string CleanHTML(string content);

        string BackgroundColor();

        string FontColor();

        void OpenIe(string url);

        void Share(string url, string title);

        void OpenSearch();

        void CancelSpeech();

        Task OpenSpeechUI();

        Task OpenTextSpeechUI(ViewModels.Guide pagesToRead);

        Task OpenBrowser(string Url);

        void ShowVideo(string Url);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Html;
using Windows.System;
using Windows.UI;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace iFixit.W8.UI.Services
{
    public class UiUx : Domain.Interfaces.IUxService
    {

        public Task LoginMessaging(string m)
        {

            return Task.Factory.StartNew(() =>
            { });
        }

        public string SanetizeHTML(string html)
        {
            html = html.Replace("&quot;", "''");
            return HtmlUtilities.ConvertToText(html);
        }

        public async Task ShowAlert(string m)
        {

            var mb = new Windows.UI.Popups.MessageDialog(m);
            await mb.ShowAsync();
        }

        public async Task ShowToast(string m)
        {
            var template = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText01);
            template.GetElementsByTagName("text")[0].InnerText = m;

            ToastNotification t = new ToastNotification(template);

            ToastNotificationManager.CreateToastNotifier().Show(t);
        }

        public string PrepHTML(string content, string BackgroundColor, string FontColor)
        {
            return content;
        }

        public string CleanHTML(string html)
        {
            html = html.Replace("&quot;", "''");
            return HtmlUtilities.ConvertToText(html);
        }

        public string BackgroundColor()
        {
            throw new NotImplementedException();
        }

        public string FontColor()
        {
            throw new NotImplementedException();
        }

        public void OpenIe(string Url)
        {
            Launcher.LaunchUriAsync(new Uri(Url));
        }

        public void Share(string url, string title)
        {
            throw new NotImplementedException();
        }

        public void OpenSearch()
        {
            Windows.ApplicationModel.Search.SearchPane a;
            a = Windows.ApplicationModel.Search.SearchPane.GetForCurrentView();
            a.Show();

        }

        public void CancelSpeech()
        {
            // throw new NotImplementedException();
        }

        public Task OpenSpeechUI()
        {
            throw new NotImplementedException();
        }

        public Task OpenTextSpeechUI(Domain.ViewModels.Guide pagesToRead)
        {
            throw new NotImplementedException();
        }

        public Task OpenBrowser(string Url)
        {
            return Task.Run(() => { OpenIe(Url); });
        }

        public void ShowVideo(string Url)
        {
            throw new NotImplementedException();
        }


        public void GoToLogin()
        {
            Windows.UI.Xaml.Controls.SettingsFlyout settings = new Windows.UI.Xaml.Controls.SettingsFlyout();
            settings.Width = 500;
            settings.HeaderBackground = new SolidColorBrush(Color.FromArgb(255, 0, 113, 206));
            settings.HeaderForeground = new SolidColorBrush(Colors.White);
            settings.Title = International.Translation.Login;
            settings.Content = new iFixit.W81.UI.Views.UC.Login();
            settings.Show();
        }

        public void GoToProfile()
        {
            Windows.UI.Xaml.Controls.SettingsFlyout settings = new Windows.UI.Xaml.Controls.SettingsFlyout();
            settings.Width = 500;
            settings.HeaderBackground = new SolidColorBrush(Color.FromArgb(255, 0, 113, 206));
            settings.HeaderForeground = new SolidColorBrush(Colors.White);
            settings.Title = International.Translation.Profile;
            settings.Content = new iFixit.W81.UI.Views.UC.Profile();
            settings.Show();
        }

        public void DoLogin()
        {
            var f = Window.Current.Content as Frame;
            f.Navigate(f.Content.GetType());
            f.GoBack();
            //  f.Focus(FocusState.Programmatic);
        }

        public void DoLogOff()
        {
            var f = Window.Current.Content as Frame;
            f.Navigate(f.Content.GetType());
            f.GoBack();
            //   f.Focus(FocusState.Programmatic);
        }
    }
}

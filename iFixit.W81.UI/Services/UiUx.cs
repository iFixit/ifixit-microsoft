using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Search;
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

            var t = new ToastNotification(template);

            ToastNotificationManager.CreateToastNotifier().Show(t);
        }

        public string PrepHTML(string content, string backgroundColor, string fontColor)
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

        public void OpenIe(string url)
        {
            Launcher.LaunchUriAsync(new Uri(url));
        }

        public void Share(string url, string title)
        {
            throw new NotImplementedException();
        }

        public void OpenSearch()
        {
            var a = SearchPane.GetForCurrentView();
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

        public Task OpenBrowser(string url)
        {
            return Task.Run(() => { OpenIe(url); });
        }

        public void ShowVideo(string url)
        {
            throw new NotImplementedException();
        }


        public void GoToLogin()
        {
            var settings = new Windows.UI.Xaml.Controls.SettingsFlyout
            {
                Width = 500,
                HeaderBackground = new SolidColorBrush(Color.FromArgb(255, 0, 113, 206)),
                HeaderForeground = new SolidColorBrush(Colors.White),
                Title = International.Translation.Login,
                Content = new iFixit.W81.UI.Views.UC.Login()
            };
            settings.Show();
        }

        public void GoToProfile()
        {
            var settings = new Windows.UI.Xaml.Controls.SettingsFlyout
            {
                Width = 500,
                HeaderBackground = new SolidColorBrush(Color.FromArgb(255, 0, 113, 206)),
                HeaderForeground = new SolidColorBrush(Colors.White),
                Title = International.Translation.Profile,
                Content = new iFixit.W81.UI.Views.UC.Profile()
            };
            settings.Show();
        }

        public void DoLogin()
        {
            var f = Window.Current.Content as Frame;
            if (f?.Content != null) f.Navigate(f.Content.GetType());
            f?.GoBack();
            //  f.Focus(FocusState.Programmatic);
        }

        public void DoLogOff()
        {
            var f = Window.Current.Content as Frame;
            if (f?.Content != null) f.Navigate(f.Content.GetType());
            f?.GoBack();
            //   f.Focus(FocusState.Programmatic);
        }
    }
}

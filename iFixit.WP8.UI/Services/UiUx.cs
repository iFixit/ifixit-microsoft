using Coding4Fun.Toolkit.Controls;
using iFixit.Domain;
using iFixit.Domain.Models.UI;
using Microsoft.Phone.Tasks;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Telerik.Windows.Controls;
using Windows.Foundation;
using Windows.Phone.Speech.Recognition;
using Windows.Phone.Speech.Synthesis;



namespace iFixit.UI.Services
{
    public class UiUx : Domain.Interfaces.IUxService
    {


       public string SanetizeHTML(string html)
        {
            return html;
        }
        public Task OpenBrowser(string Url)
        {

            return Task.Factory.StartNew(() =>
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    Cimbalino.Phone.Toolkit.Services.WebBrowserService bw = new Cimbalino.Phone.Toolkit.Services.WebBrowserService();

                    bw.Show(Url);
                });
            });


        }

        public void ShowVideo(string Url)
        {
            MediaPlayerLauncher mediaPlayerLauncher = new MediaPlayerLauncher();
            mediaPlayerLauncher.Media = new Uri(Url, UriKind.Absolute);
            mediaPlayerLauncher.Show();
        }

        public Task ShowAlert(string m)
        {
            //Cimbalino.Phone.Toolkit.Services.MessageBoxService s = new Cimbalino.Phone.Toolkit.Services.MessageBoxService();

            //return s.ShowAsync(m);

            return Task.Factory.StartNew(() =>
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    MessageBox.Show(m);
                });
            });


        }


        public async Task LoginMessaging(string m)
        {

            await ShowAlert(m);
        }
        private IAsyncAction task;
        private CancellationToken ct;
        public void CancelSpeech()
        {
            try
            {
                if (task != null)
                    task.Cancel();
            }
            catch
            {


            }

        }

        public async Task OpenTextSpeechUI(Domain.ViewModels.Guide guideToRead)
        {
            try
            {
                ObservableCollection<GuideBasePage> pagesToRead = guideToRead.Items;

                SpeechSynthesizer synth = new SpeechSynthesizer();
                var voices = InstalledVoices.All.Where(o => o.Language.StartsWith("en"));

                if (voices.Count() > 0)
                {
                    synth.SetVoice(voices.First());

                    guideToRead.BeingRead = true;


                    if (guideToRead.SelectedPageIndex == 0)
                    {
                        var toRead = ((GuideIntro)pagesToRead[0]);
                        if (!string.IsNullOrEmpty(toRead.Subject))
                        {
                            task = synth.SpeakTextAsync(toRead.Subject);
                            await task;
                        }
                        if (!string.IsNullOrEmpty(toRead.Summary))
                        {
                            task = synth.SpeakTextAsync(toRead.Summary);
                            await task;
                        }

                        guideToRead.SelectedPageIndex = 1;
                    }


                    for (int i = guideToRead.SelectedPageIndex; i < pagesToRead.Count; i++)
                    {
                        if (guideToRead.BeingRead == false)
                            break;

                        guideToRead.SelectedPage = (GuideBasePage)pagesToRead[i];

                        guideToRead.SelectedPageIndex = guideToRead.Items.IndexOf(guideToRead.SelectedPage);

                        var item = (GuideStepItem)pagesToRead[i];
                        int m = 0;

                        foreach (var line in item.Lines)
                        {
                            if (guideToRead.BeingRead == false)
                                break;

                            guideToRead.SelectedStepLine = m;
                            try
                            {
                                task = synth.SpeakTextAsync(line.VoiceText);
                                await task;
                            }
                            catch (Exception ex)
                            {

                                //  throw;
                            }
                            m++;
                        }

                    }

                    guideToRead.BeingRead = false;
                }
                else
                {
                    MessageBox.Show("please install the english voice languague pack ");
                }

            }
            catch (Exception)
            {


            }

        }

        public async Task OpenSpeechUI()
        {


            SpeechRecognizerUI recoWithUI;
            // Create an instance of SpeechRecognizerUI.
            recoWithUI = new SpeechRecognizerUI();
            var installed = InstalledSpeechRecognizers.All;
            if (installed.Any(o => o.Language == "en-US"))
            {
                recoWithUI.Recognizer.SetRecognizer(installed.Where(o => o.Language == "en-US").Single());


                // Uri searchGrammar = new Uri("ms-appx:///Assets/SRGSGrammar1.xml", UriKind.Absolute);

                // Add the SRGS grammar to the grammar set.
                //   recoWithUI.Recognizer.Grammars.AddGrammarFromUri("cities", searchGrammar);

                recoWithUI.Settings.ListenText = "search for?";
                recoWithUI.Settings.ExampleText = " 'guides', 'guide', 'device' ";
                // Start recognition (load the dictation grammar by default).

                recoWithUI.Recognizer.Grammars.AddGrammarFromPredefinedType("typeName", SpeechPredefinedGrammar.Dictation);

                SpeechRecognitionUIResult recoResult = await recoWithUI.RecognizeWithUIAsync();

                // Do something with the recognition result.
                // MessageBox.Show(string.Format("You said {0}.", recoResult.RecognitionResult.Text),);


                //  DoSearch(recoResult.RecognitionResult.Text);

            }
            else
            {
                MessageBox.Show("not lang");
            }



        }



        public Task ShowToast(string m)
        {
            return Task.Factory.StartNew(() =>
            {
                ToastPrompt toast = new ToastPrompt();
                toast.FontSize = 20;
                // toast.Title = App.AppName;
                toast.Message = m;
                // toast.ImageSource = new BitmapImage(new Uri("/ApplicationIcon.png", UriKind.RelativeOrAbsolute));
                toast.TextOrientation = System.Windows.Controls.Orientation.Horizontal;
                toast.Show();
            });
        }

        public void Share(string url, string title)
        {
            ShareLinkTask taskL = new ShareLinkTask();
            taskL.LinkUri = new Uri(url);
            taskL.Message = "";
            taskL.Title = title;
            taskL.Show();
        }

        public void OpenIe(string Url)
        {
            WebBrowserTask webBrowserTask = new WebBrowserTask { Uri = new Uri(Url, UriKind.RelativeOrAbsolute) };
            webBrowserTask.Show();
        }

        public string CleanHTML(string baseHTML)
        {
            string expn = "<.*?>";

            baseHTML = baseHTML.Replace("&nbsp;", "");
            var Result = Regex.Replace(baseHTML, expn, string.Empty);
            return HttpUtility.HtmlDecode(Result);
        }

        public string PrepHTML(string baseHTML, string BackgroundColor, string FontColor)
        {

            string expn = "<.*?>";

            baseHTML = baseHTML.Replace("&nbsp;", "");
            var Result = Regex.Replace(baseHTML, expn, string.Empty);

            if (Result.Length > 2000)
            {
                Result = Result.Substring(0, 2000) + "...";
            }

            return Result;

            //            string o = "";
            //            o += "<html><head>";

            //            //prevent zooming
            //            o += "<meta name='viewport' content='width=320,user-scalable=no'/>";

            //            //inject the theme
            //            o += "<style type='text/css'>" +
            //                "body {{font-size:1.0em;background-color:#c6bea6;" +
            //                "color:" + FontColor + ";}} " + "</style>";

            //            //inject the script to pass link taps out of the browser
            //            o += "<script type='text/javascript'>";
            //            o += @"function getLinks(){ 
            //                a = document.getElementsByTagName('a');
            //                    for(var i=0; i < a.length; i++){
            //                    var msg = a[i].href;
            //                    a[i].onclick = function() {notify(msg);
            //                    };
            //                    }
            //                    }
            //                    function notify(msg) {
            //                    window.external.Notify(msg);
            //                    event.returnValue=false;
            //                    return false;
            //                }";

            //            //inject the script to find height
            //            o += @"function Scroll() {
            //                            var elem = document.getElementById('content');
            //                            window.external.Notify(elem.scrollHeight + '');
            //                        }
            //                    ";

            //            //remove all anchors
            //            while (baseHTML.Contains("<a class=\"anchor\""))
            //            {
            //                int start = baseHTML.IndexOf("<a class=\"anchor\"");
            //                int end = baseHTML.IndexOf("</h2>", start);

            //                baseHTML = baseHTML.Remove(start, end - start);
            //            }

            //            //FIXME remove this when we fix the webbrowser
            //            //remove all links
            //            while (baseHTML.Contains("<a href"))
            //            {
            //                //remove most of the link
            //                int start = baseHTML.IndexOf("<a href");
            //                int end = baseHTML.IndexOf(">", start);

            //                baseHTML = baseHTML.Remove(start, end + 1 - start);

            //                //remove end tag
            //                start = baseHTML.IndexOf("</a>", start);
            //                baseHTML = baseHTML.Remove(start, "</a>".Length);
            //            }

            //            o += @"window.onload = function() {
            //                    Scroll();
            //                    getLinks();
            //                }";

            //            o += "</script>";
            //            o += "</head>";
            //            o += "<body><div id='content'>";
            //            o += baseHTML.Trim();
            //            o += "</div></body>";
            //            o += "</html>";
            //            return o;
        }

        public string FontColor()
        {
            return Application.Current.Resources["MainForground"].ToString();
        }

        public string BackgroundColor()
        {
            return Application.Current.Resources["MainBackground"].ToString();
        }


        public void OpenSearch()
        {
            RadInputPrompt.Show(iFixit.International.Translation.Search
                , closedHandler: (args) =>
            {
                if (!string.IsNullOrEmpty(args.Text))
                {
                    var searchTerm = args.Text;
                    DoSearch(searchTerm);
                }

            }
            , keyDownHandler: (keyArgs) =>
            {
                if (keyArgs.Key == Key.Enter)
                {
                    RadInputPrompt.Close(0);
                }
                //if (keyArgs.Key == Key.Space)
                //{
                //    RadInputPrompt.Close(1);
                //}
            }
            , vibrate: false
            );


        }

       

        private void DoSearch(string searchTerm)
        {
            ((RadPhoneApplicationFrame)Application.Current.RootVisual).Transition = new RadTurnstileTransition();

            var root = Application.Current.RootVisual as RadPhoneApplicationFrame;

            AppBase.Current.SearchTerm = searchTerm;

            root.Navigate(new Uri(UiNavigation.ViewModelRouting[typeof(Domain.ViewModels.Search)] + "?g=" + Guid.NewGuid(), UriKind.RelativeOrAbsolute));

        }



        public void GoToLogin()
        {
            Services.UiNavigation nav = new UiNavigation();
            nav.Navigate<Domain.ViewModels.Login>(false);
        }

        public void DoLogin()
        {
            Services.UiNavigation nav = new UiNavigation();
            nav.GoBack();
        }


        public void GoToProfile()
        {
            Services.UiNavigation nav = new UiNavigation();
            nav.Navigate<Domain.ViewModels.Profile>(false);
        }

        public void DoLogOff()
        {
            Services.UiNavigation nav = new UiNavigation();
            nav.GoBack();
        }
    }
}

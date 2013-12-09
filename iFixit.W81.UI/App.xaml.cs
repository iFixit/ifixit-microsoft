using iFixit.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.ApplicationSettings;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=234227

namespace iFixit.W81.UI
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        private static Windows.UI.Core.CoreDispatcher _dispatcher = Window.Current.Dispatcher;

        public static Windows.UI.Core.CoreDispatcher dispatcher
        {
            get { return App._dispatcher; }
            set { App._dispatcher = value; }
        }

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
            Domain.AppBase.Current.ExtendeInfoApp = true;
            Domain.AppBase.Current.HiResApp = true;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {

#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif

            Frame rootFrame = Window.Current.Content as Frame;
            SettingsPane.GetForCurrentView().CommandsRequested += App_CommandsRequested;
            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();
                // Set the default language
                rootFrame.Language = Windows.Globalization.ApplicationLanguages.Languages[0];

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                rootFrame.Navigate(typeof(Views.Home), e.Arguments);
            }
            // Ensure the current window is active
            Window.Current.Activate();
        }

        private void App_CommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {



            if (AppBase.Current.User == null)
            {

                SettingsCommand aboutsc = new SettingsCommand("LoginW", International.Translation.Login, (x) =>
                {



                    Windows.UI.Xaml.Controls.SettingsFlyout settings = new Windows.UI.Xaml.Controls.SettingsFlyout();
                    settings.Width = 500;
                    settings.HeaderBackground = new SolidColorBrush(Color.FromArgb(255, 0, 113, 206));
                    settings.HeaderForeground = new SolidColorBrush(Colors.White);
                    settings.Title = International.Translation.Login;
                    settings.Content = new iFixit.W81.UI.Views.UC.Login();
                    settings.Show();

                });

                args.Request.ApplicationCommands.Add(aboutsc);
            }
            else
            {

                SettingsCommand aboutsc = new SettingsCommand("ProfileFlyout", International.Translation.Profile, (x) =>
                {

                    Windows.UI.Xaml.Controls.SettingsFlyout settings = new Windows.UI.Xaml.Controls.SettingsFlyout();
                    settings.Width = 500;
                    settings.HeaderBackground = new SolidColorBrush(Color.FromArgb(255, 0, 113, 206));
                    settings.HeaderForeground = new SolidColorBrush(Colors.White);
                    settings.Title = International.Translation.Profile;
                                        settings.Content = new iFixit.W81.UI.Views.UC.Profile();
                    settings.Show();





                });

                args.Request.ApplicationCommands.Add(aboutsc);


                SettingsCommand newsCommand = new SettingsCommand("logout", International.Translation.Logout
                    , (uiCommand) => { Logout(); });
                args.Request.ApplicationCommands.Add(newsCommand);



            }
            SettingsCommand privacyPolicyCommand = new SettingsCommand("privacyPolicy", "Privacy Policy", (uiCommand) => { LaunchPrivacyPolicyUrl("http://www.ifixit.com/Info/Privacy"); });
            args.Request.ApplicationCommands.Add(privacyPolicyCommand);
        }


        async void Logout()
        {
            iFixit.Shared.UI.Services.UiStorage _storageService = new Shared.UI.Services.UiStorage();
            iFixit.W8.UI.Services.UiUx _uxService = new iFixit.W8.UI.Services.UiUx();
            iFixit.W8.UI.Services.UiSettings _settingsService = new iFixit.W8.UI.Services.UiSettings();
            Domain.Services.V2_0.ServiceBroker Broker = new Domain.Services.V2_0.ServiceBroker(_settingsService.AppKey(), _settingsService.AppVersion());
            await Domain.Code.Utils.DoLogOut(_storageService, _uxService, Broker);

            await _uxService.ShowToast(International.Translation.LogoutSuccessfull);
            _uxService.DoLogOff();
        }

        async void LaunchPrivacyPolicyUrl(string url)
        {
            Uri privacyPolicyUrl = new Uri(url);
            var result = await Windows.System.Launcher.LaunchUriAsync(privacyPolicyUrl);
        }
        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }


        protected override void OnSearchActivated(SearchActivatedEventArgs args)
        {

            base.OnSearchActivated(args);
            var previousContent = Window.Current.Content;
            var frame = previousContent as Frame;

            if (frame == null)
            {
                frame = new Frame();
            }
            dispatcher = Windows.UI.Xaml.Window.Current.Dispatcher;
            AppBase.Current.SearchTerm = args.QueryText;
            frame.Navigate(typeof(Views.SearchResult), args.QueryText);
            Window.Current.Content = frame;

            // Ensure the current window is active
            Window.Current.Activate();
        }
    }
}

using iFixit.Domain;
using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI;
using Windows.UI.ApplicationSettings;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
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

        public static Windows.UI.Core.CoreDispatcher Dispatcher
        {
            get { return _dispatcher; }
            set { _dispatcher = value; }
        }

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            InitializeComponent();
            Suspending += OnSuspending;
            AppBase.Current.ExtendeInfoApp = true;
            AppBase.Current.HiResApp = true;
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
                DebugSettings.EnableFrameRateCounter = true;
            }
#endif

            var rootFrame = Window.Current.Content as Frame;
            SettingsPane.GetForCurrentView().CommandsRequested += App_CommandsRequested;
            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame {Language = Windows.Globalization.ApplicationLanguages.Languages[0]};
                // Set the default language

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

                var aboutsc = new SettingsCommand("LoginW", International.Translation.Login, x =>
                {



                    var settings = new SettingsFlyout
                    {
                        Width = 500,
                        HeaderBackground = new SolidColorBrush(Color.FromArgb(255, 0, 113, 206)),
                        HeaderForeground = new SolidColorBrush(Colors.White),
                        Title = International.Translation.Login,
                        Content = new Views.UC.Login()
                    };
                    settings.Show();
                    

                });

                args.Request.ApplicationCommands.Add(aboutsc);
            }
            else
            {

                var aboutsc = new SettingsCommand("ProfileFlyout", International.Translation.Profile, x =>
                {

                    var settings = new SettingsFlyout
                    {
                        Width = 500,
                        HeaderBackground = new SolidColorBrush(Color.FromArgb(255, 0, 113, 206)),
                        HeaderForeground = new SolidColorBrush(Colors.White),
                        Title = International.Translation.Profile,
                        Content = new Views.UC.Profile()
                    };
                    settings.Show();





                });

                args.Request.ApplicationCommands.Add(aboutsc);


                var newsCommand = new SettingsCommand("logout", International.Translation.Logout
                    , uiCommand => { Logout(); });
                args.Request.ApplicationCommands.Add(newsCommand);



            }
            var privacyPolicyCommand = new SettingsCommand("privacyPolicy", "Privacy Policy", uiCommand => { LaunchPrivacyPolicyUrl("http://www.ifixit.com/Info/Privacy"); });
            args.Request.ApplicationCommands.Add(privacyPolicyCommand);
        }


        async void Logout()
        {
            var storageService = new Shared.UI.Services.UiStorage();
            var uxService = new W8.UI.Services.UiUx();
            var settingsService = new W8.UI.Services.UiSettings();
            var broker = new Domain.Services.V2_0.ServiceBroker(settingsService.AppKey(), settingsService.AppVersion());
            await Domain.Code.Utils.DoLogOut(storageService, uxService, broker);

            await uxService.ShowToast(International.Translation.LogoutSuccessfull);
            uxService.DoLogOff();
        }

        async void LaunchPrivacyPolicyUrl(string url)
        {
            var privacyPolicyUrl = new Uri(url);
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
            var frame = previousContent as Frame ?? new Frame();

            Dispatcher = Window.Current.Dispatcher;
            AppBase.Current.SearchTerm = args.QueryText;
            frame.Navigate(typeof(Views.SearchResult), args.QueryText);
            Window.Current.Content = frame;

            // Ensure the current window is active
            Window.Current.Activate();
        }
    }
}

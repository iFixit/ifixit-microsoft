using iFixit.Domain;
using iFixit.W8.UI.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
using Windows.UI.Xaml.Navigation;

// The Grid App template is documented at http://go.microsoft.com/fwlink/?LinkId=234226

namespace iFixit.W8.UI
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
        /// Initializes the singleton Application object.  This is the first line of authored code
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
        /// will be used when the application is launched to open a specific file, to display
        /// search results, and so forth.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override async void OnLaunched(LaunchActivatedEventArgs args)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            dispatcher = Windows.UI.Xaml.Window.Current.Dispatcher;
            SettingsPane.GetForCurrentView().CommandsRequested += App_CommandsRequested;
            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active

            if (rootFrame == null)
            {


                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();
                //Associate the frame with a SuspensionManager key                                
                SuspensionManager.RegisterFrame(rootFrame, "AppFrame");

                if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // Restore the saved session state only when appropriate
                    try
                    {
                        await SuspensionManager.RestoreAsync();
                    }
                    catch (SuspensionManagerException)
                    {
                        //Something went wrong restoring state.
                        //Assume there is no state and continue
                    }
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }
            if (rootFrame.Content == null)
            {
                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                if (!rootFrame.Navigate(typeof(Views.Home)))
                {
                    throw new Exception("Failed to create initial page");
                }
            }
            // Ensure the current window is active
            Window.Current.Activate();

            //if (System.Diagnostics.Debugger.IsAttached)
            //{
            //    //Display the metro grid helper
            //    MC.MetroGridHelper.MetroGridHelper.CreateGrid();
            //}
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            await SuspensionManager.SaveAsync();
            deferral.Complete();
        }


        void App_CommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {

            if (AppBase.Current.User == null)
            {
                Callisto.Controls.SettingsFlyout LoginFlyout = new Callisto.Controls.SettingsFlyout();
                SettingsCommand aboutsc = new SettingsCommand("LoginW", International.Translation.Login, (x) =>
                {

                    LoginFlyout.FlyoutWidth = Callisto.Controls.SettingsFlyout.SettingsFlyoutWidth.Wide;
                    LoginFlyout.HeaderText = International.Translation.Login;
                    LoginFlyout.Content = new Views.UC.Login();
                    LoginFlyout.HeaderBrush = new SolidColorBrush(Color.FromArgb(255, 0, 113, 206));

                    LoginFlyout.IsOpen = true;



                });

                args.Request.ApplicationCommands.Add(aboutsc);
            }
            else
            {
                Callisto.Controls.SettingsFlyout ProfileFlyout = new Callisto.Controls.SettingsFlyout();
                SettingsCommand aboutsc = new SettingsCommand("ProfileFlyout", International.Translation.Profile, (x) =>
                {

                    ProfileFlyout.FlyoutWidth = Callisto.Controls.SettingsFlyout.SettingsFlyoutWidth.Wide;
                    ProfileFlyout.HeaderText = International.Translation.Profile;
                    ProfileFlyout.Content = new Views.UC.Profile();
                    ProfileFlyout.HeaderBrush = new SolidColorBrush(Color.FromArgb(255, 0, 113, 206));
                    ProfileFlyout.IsOpen = true;



                });

                args.Request.ApplicationCommands.Add(aboutsc);


                SettingsCommand newsCommand = new SettingsCommand("logout", International.Translation.Logout, (uiCommand) => { Logout(); });
                args.Request.ApplicationCommands.Add(newsCommand);

            }

            //Callisto.Controls.SettingsFlyout SupportFlyout = new Callisto.Controls.SettingsFlyout();
            //args.Request.ApplicationCommands.Add(new SettingsCommand("SupportFlyout", International.Translation.Help, (x) =>
            //{

            //    SupportFlyout.FlyoutWidth = Callisto.Controls.SettingsFlyout.SettingsFlyoutWidth.Wide;
            //    SupportFlyout.HeaderText = International.Translation.Help;
            //    SupportFlyout.Content = new Views.UC.Support();
            //    SupportFlyout.IsOpen = true;



            //}));

            //Callisto.Controls.SettingsFlyout MissionFlyout = new Callisto.Controls.SettingsFlyout();
            //args.Request.ApplicationCommands.Add(new SettingsCommand("MissionFlyout", International.Translation.Mission, (x) =>
            //{

            //    //MissionFlyout.FlyoutWidth = Callisto.Controls.SettingsFlyout.SettingsFlyoutWidth.Wide;
            //    //MissionFlyout.HeaderText =  International.Translation.Mission;
            //    //MissionFlyout.Content = new Views.UC.Mission();
            //    //MissionFlyout.IsOpen = true;



            //}));

            //Callisto.Controls.SettingsFlyout NewsFlyout = new Callisto.Controls.SettingsFlyout();
            //args.Request.ApplicationCommands.Add(new SettingsCommand("NewsFlyout", International.Translation.News, (x) =>
            //{

            //    NewsFlyout.FlyoutWidth = Callisto.Controls.SettingsFlyout.SettingsFlyoutWidth.Wide;
            //    NewsFlyout.HeaderText = International.Translation.News;
            //    NewsFlyout.Content = new Views.UC.News();
            //    NewsFlyout.IsOpen = true;



            //}));
            SettingsCommand logout = new SettingsCommand("logout", International.Translation.News, (uiCommand) => { LaunchPrivacyPolicyUrl("http://ifixit.org/"); });
            args.Request.ApplicationCommands.Add(logout);

            SettingsCommand privacyPolicyCommand = new SettingsCommand("privacyPolicy", "Privacy Policy", (uiCommand) => { LaunchPrivacyPolicyUrl("http://www.ifixit.com/Info/Privacy"); });
            args.Request.ApplicationCommands.Add(privacyPolicyCommand);


        }

        async void Logout()
        {
            iFixit.Shared.UI.Services.UiStorage _storageService = new iFixit.Shared.UI.Services.UiStorage();
            iFixit.W8.UI.Services.UiUx _uxService = new Services.UiUx();
            iFixit.W8.UI.Services.UiSettings _settingsService = new Services.UiSettings();
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



        protected async override void OnSearchActivated(Windows.ApplicationModel.Activation.SearchActivatedEventArgs args)
        {
            // TODO: Register the Windows.ApplicationModel.Search.SearchPane.GetForCurrentView().QuerySubmitted
            // event in OnWindowCreated to speed up searches once the application is already running

            // If the Window isn't already using Frame navigation, insert our own Frame
            var previousContent = Window.Current.Content;
            var frame = previousContent as Frame;

            // If the app does not contain a top-level frame, it is possible that this 
            // is the initial launch of the app. Typically this method and OnLaunched 
            // in App.xaml.cs can call a common method.
            if (frame == null)
            {
                // Create a Frame to act as the navigation context and associate it with
                // a SuspensionManager key
                frame = new Frame();
                Common.SuspensionManager.RegisterFrame(frame, "AppFrame");

                if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // Restore the saved session state only when appropriate
                    try
                    {
                        await Common.SuspensionManager.RestoreAsync();
                    }
                    catch (Common.SuspensionManagerException)
                    {
                        //Something went wrong restoring state.
                        //Assume there is no state and continue
                    }
                }
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

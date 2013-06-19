using System;
using System.Diagnostics;
using System.Resources;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using iFixit.WP8.UI.Resources;
using Telerik.Windows.Controls;
using iFixit.WP8.UI.Code;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Linq;

namespace iFixit.WP8.UI
{
    public partial class App : Application
    {
        /// <summary>
        /// Component used to handle unhandle exceptions, to collect runtime info and to send email to developer.
        /// </summary>
        public RadDiagnostics diagnostics;
        /// <summary>
        /// Component used to raise a notification to the end users to rate the application on the marketplace.
        /// </summary>
        public RadRateApplicationReminder rateReminder;


        /// <summary>
        /// used for Fast resume
        /// </summary>
        private bool reset;

        /// <summary>
        /// Provides easy access to the root frame of the Phone Application.
        /// </summary>
        /// <returns>The root frame of the Phone Application.</returns>
        public static PhoneApplicationFrame RootFrame { get; private set; }

        /// <summary>
        /// Constructor for the Application object.
        /// </summary>
        public App()
        {
            // Global handler for uncaught exceptions.
            UnhandledException += Application_UnhandledException;

            // Standard XAML initialization
            InitializeComponent();

            // Phone-specific initialization
            InitializePhoneApplication();

            // Language display initialization
            InitializeLanguage();

            // Show graphics profiling information while debugging.
            if (Debugger.IsAttached)
            {
                // Display the current frame rate counters.
                Application.Current.Host.Settings.EnableFrameRateCounter = true;

                // Show the areas of the app that are being redrawn in each frame.
                //Application.Current.Host.Settings.EnableRedrawRegions = true;

                // Enable non-production analysis visualization mode,
                // which shows areas of a page that are handed off to GPU with a colored overlay.
                //Application.Current.Host.Settings.EnableCacheVisualization = true;

                // Prevent the screen from turning off while under the debugger by disabling
                // the application's idle detection.
                // Caution:- Use this under debug mode only. Application that disables user idle detection will continue to run
                // and consume battery power when the user is not using the phone.
                PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
            }



            //Creates an instance of the Diagnostics component.
            diagnostics = new RadDiagnostics();

            //Defines the default email where the diagnostics info will be send.
            diagnostics.EmailTo = "ideiasemmovimento@outlook.com";

            //Initializes this instance.
            diagnostics.Init();

            //Creates a new instance of the RadRateApplicationReminder component.
            rateReminder = new RadRateApplicationReminder();

            //Sets how often the rate reminder is displayed.
            rateReminder.RecurrencePerUsageCount = 20;
            ThemeManager.ToLightTheme();
        }

        // Code to execute when the application is launching (eg, from Start)
        // This code will not execute when the application is reactivated
        private void Application_Launching(object sender, LaunchingEventArgs e)
        {
            ApplicationUsageHelper.Init("1.0");
            // LowMemoryHelper.BeginRecording();
        }

        // Code to execute when the application is activated (brought to foreground)
        // This code will not execute when the application is first launched
        private void Application_Activated(object sender, ActivatedEventArgs e)
        {
            if (!e.IsApplicationInstancePreserved)
            {
                //This will ensure that the ApplicationUsageHelper is initialized again if the application has been in Tombstoned state.
                ApplicationUsageHelper.OnApplicationActivated();
            }
        }

        // Code to execute when the application is deactivated (sent to background)
        // This code will not execute when the application is closing
        private void Application_Deactivated(object sender, DeactivatedEventArgs e)
        {
        }

        // Code to execute when the application is closing (eg, user hit Back)
        // This code will not execute when the application is deactivated
        private void Application_Closing(object sender, ClosingEventArgs e)
        {
        }

        // Code to execute if a navigation fails
        private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                // A navigation has failed; break into the debugger
                Debugger.Break();
            }
        }

        // Code to execute on Unhandled Exceptions
        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                Debugger.Break();
            }
        }

        #region Phone application initialization

        // Avoid double-initialization
        private bool phoneApplicationInitialized = false;

        // Do not add any additional code to this method
        private void InitializePhoneApplication()
        {
            if (phoneApplicationInitialized)
                return;

            //TODO: transition image

            int ImageSize = 1280;
            Uri image = new Uri("Assets/SplashScreenImage-WXGA.png", UriKind.RelativeOrAbsolute);
            switch (ResolutionHelper.CurrentResolution)
            {
                case Resolutions.HD720p:
                    image = new Uri("Assets/SplashScreenImage-720p.png", UriKind.RelativeOrAbsolute);
                    ImageSize = 1280;
                    break;
                case Resolutions.WXGA:
                    image = new Uri("Assets/SplashScreenImage-WXGA.png", UriKind.RelativeOrAbsolute);
                    ImageSize = 1280;
                    break;
                case Resolutions.WVGA:
                    image = new Uri("Assets/SplashScreenImage-WVGA.png", UriKind.RelativeOrAbsolute);
                    ImageSize = 800;
                    break;
                default:
                    ImageSize = 800;
                    break;
            }



            // Create the frame but don't set it as RootVisual yet; this allows the splash
            // screen to remain active until the application is ready to render.
            RootFrame = new RadPhoneApplicationFrame();
            //RootFrame = new RadPhoneApplicationFrame
            //{
            //    Background = new ImageBrush
            //    {
            //        ImageSource = new BitmapImage(new Uri("Assets/Frame-BG.png", UriKind.RelativeOrAbsolute)),
            //        Opacity = 1,
            //        Stretch = Stretch.UniformToFill
            //    }
            //    ,
            //    Height = ImageSize
            //};
            RootFrame.Navigated += CompleteInitializePhoneApplication;

            // Handle navigation failures
            RootFrame.NavigationFailed += RootFrame_NavigationFailed;

            // Handle reset requests for clearing the backstack
            RootFrame.Navigated += CheckForResetNavigation;

            // Ensure we don't initialize again
            phoneApplicationInitialized = true;

            RootFrame.Navigating += RootFrame_Navigating;
            RootFrame.Navigated += RootFrame_Navigated;

            SetTile();
        }

        public static void SetTile()
        {

            if (ShellTile.ActiveTiles.Any())
            {
                var tile = ShellTile.ActiveTiles.First();
                
                var flipTileData = new RadFlipTileData
                {
                    IsTransparencySupported = true,
                    BackTitle = "iFixit",  
                    BackBackgroundImage = new Uri("/Assets/Tiles/Medium/3.png", UriKind.RelativeOrAbsolute),
                    WideBackBackgroundImage = new Uri("/Assets/Tiles/Large/3.png", UriKind.RelativeOrAbsolute)
                };
                LiveTileHelper.UpdateTile(tile, flipTileData);
            }



        }

        void RootFrame_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            string Home = "home";
            if (reset && e.IsCancelable && e.Uri.OriginalString.ToLower().Contains(Home))
            {
                e.Cancel = true;
                reset = false;
            }
        }

        void RootFrame_Navigated(object sender, NavigationEventArgs e)
        {
            reset = e.NavigationMode == NavigationMode.Reset;
        }

        // Do not add any additional code to this method
        private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
        {
            // Set the root visual to allow the application to render
            if (RootVisual != RootFrame)
                RootVisual = RootFrame;

            // Remove this handler since it is no longer needed
            RootFrame.Navigated -= CompleteInitializePhoneApplication;
        }

        private void CheckForResetNavigation(object sender, NavigationEventArgs e)
        {
            // If the app has received a 'reset' navigation, then we need to check
            // on the next navigation to see if the page stack should be reset
            if (e.NavigationMode == NavigationMode.Reset)
                RootFrame.Navigated += ClearBackStackAfterReset;
        }

        private void ClearBackStackAfterReset(object sender, NavigationEventArgs e)
        {
            // Unregister the event so it doesn't get called again
            RootFrame.Navigated -= ClearBackStackAfterReset;

            // Only clear the stack for 'new' (forward) and 'refresh' navigations
            if (e.NavigationMode != NavigationMode.New && e.NavigationMode != NavigationMode.Refresh)
                return;

            // For UI consistency, clear the entire page stack
            while (RootFrame.RemoveBackEntry() != null)
            {
                ; // do nothing
            }
        }

        #endregion

        // Initialize the app's font and flow direction as defined in its localized resource strings.
        //
        // To ensure that the font of your application is aligned with its supported languages and that the
        // FlowDirection for each of those languages follows its traditional direction, ResourceLanguage
        // and ResourceFlowDirection should be initialized in each resx file to match these values with that
        // file's culture. For example:
        //
        // AppResources.es-ES.resx
        //    ResourceLanguage's value should be "es-ES"
        //    ResourceFlowDirection's value should be "LeftToRight"
        //
        // AppResources.ar-SA.resx
        //     ResourceLanguage's value should be "ar-SA"
        //     ResourceFlowDirection's value should be "RightToLeft"
        //
        // For more info on localizing Windows Phone apps see http://go.microsoft.com/fwlink/?LinkId=262072.
        //
        private void InitializeLanguage()
        {
            try
            {
                // Set the font to match the display language defined by the
                // ResourceLanguage resource string for each supported language.
                //
                // Fall back to the font of the neutral language if the Display
                // language of the phone is not supported.
                //
                // If a compiler error is hit then ResourceLanguage is missing from
                // the resource file.
                RootFrame.Language = XmlLanguage.GetLanguage(AppResources.ResourceLanguage);

                // Set the FlowDirection of all elements under the root frame based
                // on the ResourceFlowDirection resource string for each
                // supported language.
                //
                // If a compiler error is hit then ResourceFlowDirection is missing from
                // the resource file.
                FlowDirection flow = (FlowDirection)Enum.Parse(typeof(FlowDirection), AppResources.ResourceFlowDirection);
                RootFrame.FlowDirection = flow;
            }
            catch
            {
                // If an exception is caught here it is most likely due to either
                // ResourceLangauge not being correctly set to a supported language
                // code or ResourceFlowDirection is set to a value other than LeftToRight
                // or RightToLeft.

                if (Debugger.IsAttached)
                {
                    Debugger.Break();
                }

                throw;
            }
        }


    }
}
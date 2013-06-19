/*
 * Taken from: http://www.scottlogic.co.uk/blog/colin/2011/11/suppressing-zoom-and-scroll-interactions-in-the-windows-phone-7-browser-control/
 */

using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using LinqToVisualTree;
using Microsoft.Phone.Controls;
using System.Diagnostics;

/// <summary>
/// Suppresses pinch zoom and optionally scrolling of the WebBrowser control
/// </summary>
/// 

namespace iFixit.UI
{
    public class WebBrowserHelper
    {
        private WebBrowser _browser;


        public static readonly DependencyProperty HtmlProperty = DependencyProperty.RegisterAttached(
            "Html", typeof(string), typeof(WebBrowserHelper), new PropertyMetadata(OnHtmlChanged));

        public static string GetHtml(DependencyObject dependencyObject)
        {
            return (string)dependencyObject.GetValue(HtmlProperty);
        }

        public static void SetHtml(DependencyObject dependencyObject, string value)
        {
            dependencyObject.SetValue(HtmlProperty, value);
        }

        private static void OnHtmlChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var browser = d as WebBrowser;

            if (browser == null)
                return;

            var html = e.NewValue.ToString();

            browser.NavigateToString(html);
        }

        /// <summary>
        /// Gets or sets whether to suppress the scrolling of
        /// the WebBrowser control;
        /// </summary>
        public bool ScrollDisabled { get; set; }

        public WebBrowserHelper(WebBrowser browser)
        {
            _browser = browser;
            browser.Loaded += new RoutedEventHandler(browser_Loaded);
        }

        private void browser_Loaded(object sender, RoutedEventArgs e)
        {
            var border = _browser.Descendants<Border>().Last() as Border;

            border.ManipulationDelta += Border_ManipulationDelta;
            border.ManipulationCompleted += Border_ManipulationCompleted;
        }

        private void Border_ManipulationCompleted(object sender,
                                                  ManipulationCompletedEventArgs e)
        {
            // suppress zoom
            if (e.FinalVelocities.ExpansionVelocity.X != 0.0 ||
                e.FinalVelocities.ExpansionVelocity.Y != 0.0)
                e.Handled = true;
        }

        private void Border_ManipulationDelta(object sender,
                                              ManipulationDeltaEventArgs e)
        {
            // suppress zoom
            if (e.DeltaManipulation.Scale.X != 0.0 ||
                e.DeltaManipulation.Scale.Y != 0.0)
                e.Handled = true;

            // optionally suppress scrolling
            if (ScrollDisabled)
            {
                if (e.DeltaManipulation.Translation.X != 0.0 ||
                  e.DeltaManipulation.Translation.Y != 0.0)
                    e.Handled = true;
            }
        }
    }
}


using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace iFixit.WP8.UI.Code
{
    public class KeyboardHelper
    {
        private List<Control> tabbedControls;
        private Control lastTabbedControl = null;
        private Panel layoutRoot;
        private PhoneApplicationPage page;

        /// <summary>
        /// Constructor to support a phone page
        /// </summary>
        /// <param name="page"></param>
        /// <param name="layoutRoot"></param>
        public KeyboardHelper(PhoneApplicationPage page, Panel layoutRoot)
        {
            this.layoutRoot = layoutRoot;
            this.page = page;
            RefeshTabbedControls(layoutRoot);
        }

        /// <summary>
        /// Refresh the tabbed controls collection, helpful if you dynamically alter the controls
        /// </summary>
        /// <param name="layoutRoot"></param>
        public void RefeshTabbedControls(Panel layoutRoot)
        {
            this.tabbedControls = GetChildsRecursive(layoutRoot).OfType<Control>().Where(c => c.IsTabStop && c.TabIndex <= int.MaxValue).OrderBy(c => c.TabIndex).ToList();
            if (this.tabbedControls != null && this.tabbedControls.Count > 0)
            {
                this.lastTabbedControl = this.tabbedControls[this.tabbedControls.Count - 1];
            }
        }

        // code from 'tucod'
        IEnumerable<DependencyObject> GetChildsRecursive(DependencyObject root)
        {
            List<DependencyObject> elts = new List<DependencyObject>();
            elts.Add(root);
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(root); i++)
                elts.AddRange(GetChildsRecursive(VisualTreeHelper.GetChild(root, i)));

            return elts;
        }

        /// <summary>
        /// Process a return key from the client controls key-up event
        /// </summary>
        /// <param name="sender"></param>
        internal void HandleReturnKey(object sender)
        {
            Control eventSender = sender as Control;
            if (eventSender != null)
            {
                Control thisControlInTabbedList = tabbedControls.FirstOrDefault(c => c == eventSender);
                if (thisControlInTabbedList != null)
                {
                    // what's the next control?                                                                       
                    SetFocusOnNextControl(thisControlInTabbedList);
                }
            }
        }

        private void SetFocusOnNextControl(Control thisControlInTabbedList)
        {
            if (lastTabbedControl == thisControlInTabbedList)
            {
                // we've come the end so remove the keyboard
                this.page.Focus();
            }
            else
            {
                Control nextControl = tabbedControls.FirstOrDefault(c => c.TabIndex > thisControlInTabbedList.TabIndex);
                bool wasFocusSet = false;
                if (nextControl != null)
                {
                    wasFocusSet = nextControl.Focus();
                }

                if (!wasFocusSet)
                {
                    SetFocusOnNextControl(nextControl);
                }

            }
        }
    }
}

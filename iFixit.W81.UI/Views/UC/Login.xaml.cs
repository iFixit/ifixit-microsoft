using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ApplicationSettings;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace iFixit.W81.UI.Views.UC
{
    public sealed partial class Login : UserControl
    {

        Domain.ViewModels.Login vm;
        public Login()
        {
            this.InitializeComponent();
            this.Loaded += Login_Loaded;
          
        }

      

       

        void Login_Loaded(object sender, RoutedEventArgs e)
        {
            vm = (Domain.ViewModels.Login)this.DataContext;

            vm.PropertyChanged += vm_PropertyChanged;
        }

        void vm_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsAuthenticated")
            {
                if (vm.IsAuthenticated == true)
                {
                    var x = (Windows.UI.Xaml.Controls.SettingsFlyout)this.Parent;
                    x.Hide();
                }
            }
        }
    }
}

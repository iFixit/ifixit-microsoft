using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

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

using Windows.UI.Xaml.Navigation;

namespace iFixit.W81.UI.Common
{
    public class BasePage : Windows.UI.Xaml.Controls.Page
    {
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var viewModel = this.DataContext as Domain.ViewModels.BaseViewModel;
            string p = e.Parameter != null ? e.Parameter.ToString() : "";// as string;
            viewModel.NavigationParameterJson = p;
            viewModel.CanGoBack = this.Frame.CanGoBack;
            switch (e.NavigationMode)
            {
                case Windows.UI.Xaml.Navigation.NavigationMode.Back:
                    viewModel.NavigationType = Domain.Interfaces.NavigationModes.Back;
                    break;
                case Windows.UI.Xaml.Navigation.NavigationMode.Forward:
                    viewModel.NavigationType = Domain.Interfaces.NavigationModes.Forward;
                    break;
                case Windows.UI.Xaml.Navigation.NavigationMode.New:
                    viewModel.NavigationType = Domain.Interfaces.NavigationModes.New;
                    break;
                case Windows.UI.Xaml.Navigation.NavigationMode.Refresh:
                    viewModel.NavigationType = Domain.Interfaces.NavigationModes.Refresh;
                    break;

                default:
                    break;
            }

            base.OnNavigatedTo(e);
        }
    }
}

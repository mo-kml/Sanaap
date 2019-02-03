using Bit.ViewModel;
using Bit.ViewModel.Contracts;
using Sanaap.App.Views;
using Syncfusion.SfNavigationDrawer.XForms;
using Xamarin.Forms;

namespace Sanaap.App.Controls.ViewModels
{
    public class MenuViewModel : BitViewModelBase
    {
        public BitDelegateCommand<string> GoToPage { get; set; }

        public BitDelegateCommand Logout { get; set; }

        public BitDelegateCommand GoBack { get; set; }

        public SfNavigationDrawer NavigationDrawer { get; set; }

        public MenuViewModel(ISecurityService securityService)
        {
            GoToPage = new BitDelegateCommand<string>(async (page) =>
            {
                NavigationDrawer.ToggleDrawer();
                await NavigationService.NavigateAsync(page);
            });

            Logout = new BitDelegateCommand(async () =>
            {
                NavigationDrawer.ToggleDrawer();
                await securityService.Logout();
                await NavigationService.NavigateAsync($"/{nameof(NavigationPage)}/{nameof(LoginView)}");
            });

            GoBack = new BitDelegateCommand(async () =>
              {
                  NavigationDrawer.ToggleDrawer();
                  await NavigationService.GoBackAsync();
              });

        }
        public BitDelegateCommand Submit { get; set; }

        public void Dispose()
        {

        }
    }
}

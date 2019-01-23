using Bit.ViewModel;
using Bit.ViewModel.Contracts;
using Prism.Ioc;
using PropertyChanged;
using Sanaap.App.Views;
using Xamarin.Forms;

namespace Sanaap.App.Controls.ViewModels
{
    public class MenuViewModelLocator
    {
        public MenuViewModel MenuViewModel => ((App)Application.Current).Container.Resolve<MenuViewModel>();
    }

    [AddINotifyPropertyChangedInterface]
    public class MenuViewModel
    {
        public BitDelegateCommand<string> GoToPage { get; set; }

        public BitDelegateCommand Logout { get; set; }


        public BitDelegateCommand GoBack { get; set; }

        public MenuViewModel(ISecurityService securityService, INavService navService)
        {
            GoToPage = new BitDelegateCommand<string>(async (page) =>
            {
                //NavigationDrawer.ToggleDrawer();
                await navService.NavigateAsync(page);
            });

            Logout = new BitDelegateCommand(async () =>
            {
                //NavigationDrawer.ToggleDrawer();
                await securityService.Logout();
                await navService.NavigateAsync($"/{nameof(NavigationPage)}/{nameof(LoginView)}");
            });

            GoBack = new BitDelegateCommand(async () =>
              {

                  //NavigationDrawer.ToggleDrawer();
                  await navService.GoBackAsync();
              });

        }
        public BitDelegateCommand Submit { get; set; }

        public void Dispose()
        {

        }
    }
}

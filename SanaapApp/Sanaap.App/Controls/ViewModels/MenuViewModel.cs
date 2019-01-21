using Bit.ViewModel;
using Bit.ViewModel.Contracts;
using Prism.Ioc;
using Prism.Navigation;
using Sanaap.App.Views;
using Syncfusion.SfNavigationDrawer.XForms;

namespace Sanaap.App.Controls.ViewModels
{
    public class MenuViewModelLocator
    {
        public MenuViewModel MenuViewModel => ((App)Xamarin.Forms.Application.Current).Container.Resolve<MenuViewModel>();
    }

    public class MenuViewModel : BitViewModelBase
    {
        public BitDelegateCommand<string> GoToPage { get; set; }

        public BitDelegateCommand Logout { get; set; }


        public BitDelegateCommand GoBack { get; set; }

        public SfNavigationDrawer NavigationDrawer { get; set; }

        public MenuViewModel( ISecurityService securityService)
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
                await NavigationService.NavigateAsync($"/{nameof(LoginView)}");
            });

            GoBack = new BitDelegateCommand(async () =>
              {
                  NavigationDrawer.ToggleDrawer();
                  await NavigationService.GoBackAsync();
              });

        }
        public BitDelegateCommand Submit { get; set; }
    }
}

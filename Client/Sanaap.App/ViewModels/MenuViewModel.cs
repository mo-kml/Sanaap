using Bit.ViewModel;
using Prism.Navigation;

namespace Sanaap.App.ViewModels
{
    public class MenuViewModel : BitViewModelBase
    {
        public BitDelegateCommand GoToMySosRequests { get; set; }

        public BitDelegateCommand Logout { get; set; }

        public MenuViewModel(INavigationService navigationService)
        {
            GoToMySosRequests = new BitDelegateCommand(async () =>
            {
                await navigationService.NavigateAsync("Menu/MySosRequests");
            });

            Logout = new BitDelegateCommand(async () =>
            {
                IsBusy = true;
                try
                {
                    await securityService.Logout();
                    await navigationService.NavigateAsync("/Login");
                }
                finally
                {
                    IsBusy = false;
                }
            });
        }
    }
}
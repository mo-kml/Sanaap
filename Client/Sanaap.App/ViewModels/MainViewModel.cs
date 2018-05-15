using Bit.ViewModel;
using Bit.ViewModel.Contracts;
using Prism.Navigation;

namespace Sanaap.App.ViewModels
{
    public class MainViewModel : BitViewModelBase
    {
        public BitDelegateCommand Logout { get; set; }

        public BitDelegateCommand SubmitEvlRequest { get; set; }

        public BitDelegateCommand MyEvlRequests { get; set; }

        public bool IsBusy { get; set; } = false;

        public MainViewModel(INavigationService navigationService, ISecurityService securityService)
        {
            Logout = new BitDelegateCommand(async () =>
            {
                IsBusy = true;
                await securityService.Logout();
                await navigationService.NavigateAsync("/Login");
                IsBusy = false;
            });

            SubmitEvlRequest = new BitDelegateCommand(async () =>
            {
                await navigationService.NavigateAsync("SubmitEvlRequest");
            });

            MyEvlRequests = new BitDelegateCommand(async () =>
            {
                await navigationService.NavigateAsync("MyEvlRequests");
            });
        }
    }
}

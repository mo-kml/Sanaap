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

        public MainViewModel(INavigationService navigationService, ISecurityService securityService)
        {
            Logout = new BitDelegateCommand(async () =>
            {
                await securityService.Logout();
                await navigationService.NavigateAsync("/Login");
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

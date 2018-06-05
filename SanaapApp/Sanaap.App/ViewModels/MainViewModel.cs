using Bit.ViewModel;
using Bit.ViewModel.Contracts;
using Prism.Navigation;
using Prism.Services;
using System;

namespace Sanaap.App.ViewModels
{
    public class MainViewModel : BitViewModelBase
    {
        public BitDelegateCommand GoToMySosRequests { get; set; }

        public BitDelegateCommand Logout { get; set; }

        public BitDelegateCommand SubmitSosRequest { get; set; }

        public BitDelegateCommand SubmitSosRequestByCall { get; set; }

        public bool IsBusy { get; set; } = false;

        public MainViewModel(INavigationService navigationService,
            ISecurityService securityService,
            IDeviceService deviceService)
        {
            GoToMySosRequests = new BitDelegateCommand(async () =>
            {
                await navigationService.NavigateAsync("MySosRequests");
            });

            Logout = new BitDelegateCommand(async () =>
            {
                await securityService.Logout();
                await navigationService.NavigateAsync("/Login");
            });

            SubmitSosRequest = new BitDelegateCommand(async () =>
            {
                await navigationService.NavigateAsync("SubmitSosRequest");
            });

            SubmitSosRequestByCall = new BitDelegateCommand(() =>
            {
                deviceService.OpenUri(new Uri("tel://0211401"));
            });
        }
    }
}

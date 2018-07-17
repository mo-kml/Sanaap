using Bit.ViewModel;
using Bit.ViewModel.Contracts;
using Prism.Navigation;
using Prism.Services;
using System;

namespace Sanaap.App.ViewModels
{
    public class SosRequestViewModel : BitViewModelBase
    {
        public BitDelegateCommand GoToMySosRequests { get; set; }

        public BitDelegateCommand Logout { get; set; }

        public BitDelegateCommand SubmitSosRequest { get; set; }

        public BitDelegateCommand GoToEvlRequest { get; set; }

        public BitDelegateCommand SubmitSosRequestByCall { get; set; }

        public SosRequestViewModel(INavigationService navigationService,
            ISecurityService securityService, IDeviceService deviceService)
        {
            GoToMySosRequests = new BitDelegateCommand(async () =>
            {
                await navigationService.NavigateAsync("MySosRequests");
            });

            GoToEvlRequest = new BitDelegateCommand(async () =>
            {
                await navigationService.NavigateAsync("EvlRequest");
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

            SubmitSosRequestByCall = new BitDelegateCommand(async () =>
            {
                deviceService.OpenUri(new Uri("tel://0211401"));
            });
        }
    }
}

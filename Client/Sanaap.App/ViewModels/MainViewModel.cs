using Bit.ViewModel;
using Bit.ViewModel.Contracts;
using Prism.Navigation;
using Prism.Services;
using System;

namespace Sanaap.App.ViewModels
{
    public class MainViewModel : BitViewModelBase
    {
        public BitDelegateCommand Logout { get; set; }

        public BitDelegateCommand SubmitSosRequest { get; set; }

        public BitDelegateCommand SubmitSosRequestByCall { get; set; }

        public BitDelegateCommand GoToMySosRequests { get; set; }

        public bool IsBusy { get; set; } = false;

        public MainViewModel(INavigationService navigationService,
            ISecurityService securityService,
            IDeviceService deviceService)
        {
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

            SubmitSosRequest = new BitDelegateCommand(async () =>
            {
                await navigationService.NavigateAsync("SubmitSosRequest");
            });

            SubmitSosRequestByCall = new BitDelegateCommand(() =>
            {
                deviceService.OpenUri(new Uri("tel://02141558"));
            });

            GoToMySosRequests = new BitDelegateCommand(async () =>
            {
                await navigationService.NavigateAsync("MySosRequests");
            });
        }
    }
}
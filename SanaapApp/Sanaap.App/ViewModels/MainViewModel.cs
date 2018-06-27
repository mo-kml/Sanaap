using Acr.UserDialogs;
using Bit.ViewModel;
using Bit.ViewModel.Contracts;
using Prism.Navigation;
using Prism.Services;
using Sanaap.Constants;
using System;

namespace Sanaap.App.ViewModels
{
    public class MainViewModel : BitViewModelBase
    {
        public BitDelegateCommand GoToMySosRequests { get; set; }

        public BitDelegateCommand Logout { get; set; }

        public BitDelegateCommand SosRequest { get; set; }

        public BitDelegateCommand EvlExpertRequest { get; set; }

        public BitDelegateCommand SubmitSosRequestByCall { get; set; }

        public bool IsBusy { get; set; } = false;

        public MainViewModel(INavigationService navigationService,
            ISecurityService securityService, IDeviceService deviceService, IUserDialogs userDialogs)
        {
            using (userDialogs.Loading(ConstantStrings.Loading))
            {
                GoToMySosRequests = new BitDelegateCommand(async () =>
            {
                await navigationService.NavigateAsync("MySosRequests");
            });

                EvlExpertRequest = new BitDelegateCommand(async () =>
                {
                    await navigationService.NavigateAsync("EvlExpertRequest");
                });

                Logout = new BitDelegateCommand(async () =>
                {
                    await securityService.Logout();
                    await navigationService.NavigateAsync("/Login");
                });

                SosRequest = new BitDelegateCommand(async () =>
                {
                    await navigationService.NavigateAsync("SosRequest");
                });

                SubmitSosRequestByCall = new BitDelegateCommand(() =>
                {
                    deviceService.OpenUri(new Uri("tel://0211401"));
                });
            }
        }
    }
}

using Acr.UserDialogs;
using Bit.ViewModel;
using Bit.ViewModel.Contracts;
using Prism.Navigation;
using Prism.Services;
using Sanaap.Enums;
using System;

namespace Sanaap.App.ViewModels
{
    public class MainInsuranceViewModel : BitViewModelBase
    {
        public BitDelegateCommand GoToMySosRequests { get; set; }

        public BitDelegateCommand Logout { get; set; }

        public BitDelegateCommand SosRequest { get; set; }

        public BitDelegateCommand GotoEvlRequestMapSales { get; set; }
        public BitDelegateCommand GotoEvlRequestMapBadane { get; set; }
        public BitDelegateCommand GotoDetail { get; set; }

        public BitDelegateCommand SubmitSosRequestByCall { get; set; }

        public MainInsuranceViewModel(INavigationService navigationService,
            ISecurityService securityService, IDeviceService deviceService, IUserDialogs userDialogs)
        {
            GoToMySosRequests = new BitDelegateCommand(async () =>
            {
                await navigationService.NavigateAsync("MySosRequests");
            });

            GotoEvlRequestMapSales = new BitDelegateCommand(async () =>
            {
                await navigationService.NavigateAsync("EvlRequestMap", new NavigationParameters
                {
                    { "InsuranceType", InsuranceType.Sales }
                });
            });

            GotoEvlRequestMapBadane = new BitDelegateCommand(async () =>
            {
                await navigationService.NavigateAsync("EvlRequestMap", new NavigationParameters
                {
                    { "InsuranceType", InsuranceType.Badane }
                });
            });

            GotoDetail = new BitDelegateCommand(async () =>
            {
                await navigationService.NavigateAsync("EvlRequestDetail");
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

            SubmitSosRequestByCall = new BitDelegateCommand(async () =>
            {
                deviceService.OpenUri(new Uri("tel://0211401"));
            });
        }
    }
}

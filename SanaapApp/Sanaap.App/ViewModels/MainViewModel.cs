﻿using Acr.UserDialogs;
using Bit.ViewModel;
using Bit.ViewModel.Contracts;
using Prism.Navigation;
using Prism.Services;
using Sanaap.Enums;
using System;

namespace Sanaap.App.ViewModels
{
    public class MainViewModel : BitViewModelBase
    {
        public BitDelegateCommand GoToMySosRequests { get; set; }

        public BitDelegateCommand Logout { get; set; }

        public BitDelegateCommand SosRequest { get; set; }

        public BitDelegateCommand GotoMainInsurance { get; set; }
        public BitDelegateCommand GotoEvlRequestMapBadane { get; set; }
        public BitDelegateCommand GotoDetail { get; set; }

        public BitDelegateCommand SubmitSosRequestByCall { get; set; }

        public MainViewModel(INavigationService navigationService,
            ISecurityService securityService, IDeviceService deviceService, IUserDialogs userDialogs)
        {
            GoToMySosRequests = new BitDelegateCommand(async () =>
            {
                await navigationService.NavigateAsync("MySosRequests");
            });

            GotoMainInsurance = new BitDelegateCommand(async () =>
            {
                await navigationService.NavigateAsync("MainInsurance");
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

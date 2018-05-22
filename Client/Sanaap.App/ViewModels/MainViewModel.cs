using Bit.ViewModel;
using Bit.ViewModel.Contracts;
using Prism.Navigation;
using Prism.Services;
using System;
using Xamarin.Forms;

namespace Sanaap.App.ViewModels
{
    public class MainViewModel : BitViewModelBase
    {
        public BitDelegateCommand Logout { get; set; }

        public BitDelegateCommand SubmitEvlRequest { get; set; }

        public BitDelegateCommand SubmitEvlRequestByCall { get; set; }

        public BitDelegateCommand GoToMyEvlRequests { get; set; }

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

            SubmitEvlRequest = new BitDelegateCommand(async () =>
            {
                await navigationService.NavigateAsync("SubmitEvlRequest");
            });

            SubmitEvlRequestByCall = new BitDelegateCommand(() =>
            {
                deviceService.OpenUri(new Uri("tel://02141558"));
            });

            GoToMyEvlRequests = new BitDelegateCommand(async () =>
            {
                await navigationService.NavigateAsync("MyEvlRequests");
            });
        }
    }
}
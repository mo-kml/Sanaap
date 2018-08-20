using Acr.UserDialogs;
using Bit.ViewModel;
using Bit.ViewModel.Contracts;
using Prism.Navigation;
using Prism.Services;
using Sanaap.Enums;

namespace Sanaap.App.ViewModels
{
    public class MainInsuranceViewModel : BitViewModelBase
    {
        public BitDelegateCommand GotoEvlRequestMapSales { get; set; }

        public BitDelegateCommand GotoEvlRequestMapBadane { get; set; }


        public MainInsuranceViewModel(INavigationService navigationService,
            ISecurityService securityService, IDeviceService deviceService, IUserDialogs userDialogs)
        {
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
        }
    }
}

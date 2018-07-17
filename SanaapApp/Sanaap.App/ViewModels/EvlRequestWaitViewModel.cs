using Bit.ViewModel;
using Prism.Navigation;
using Prism.Services;
using Simple.OData.Client;
using System;
using System.Net.Http;

namespace Sanaap.App.ViewModels
{
    public class EvlRequestWaitViewModel : BitViewModelBase, IDestructible
    {
        private readonly HttpClient _httpClient;
        private readonly IODataClient _odataClient;

        public string ExpertFullName { get; set; }
        public string ExpertMobileNo { get; set; }

        public BitDelegateCommand GoToMain { get; set; }
        public BitDelegateCommand Call { get; set; }

        public bool IsVisibleBefore { get; set; } = true;
        public bool IsVisibleAfter { get; set; } = false;

        public string Message { get; set; }

        public EvlRequestWaitViewModel(INavigationService navigationService,
            HttpClient httpClient,
            IODataClient odataClient,
            IDeviceService deviceService,
            IODataClient oDataClient)
        {
            _httpClient = httpClient;
            _odataClient = odataClient;

            GoToMain = new BitDelegateCommand(async () =>
            {
                await navigationService.NavigateAsync("/Menu/Nav/Main");
            });

            Call = new BitDelegateCommand(async () =>
            {
                deviceService.OpenUri(new Uri("tel://" + ExpertMobileNo + ""));
            });
        }
    }
}

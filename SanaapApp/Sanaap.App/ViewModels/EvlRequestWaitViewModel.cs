using Bit.ViewModel;
using Prism.Navigation;
using Prism.Services;
using Sanaap.App.Dto;
using Sanaap.Dto;
using Simple.OData.Client;
using System;
using System.Threading.Tasks;

namespace Sanaap.App.ViewModels
{
    public class EvlRequestWaitViewModel : BitViewModelBase, IDestructible
    {
        private readonly IODataClient _odataClient;
        private EvlRequestDto _evlRequest;

        public string ExpertFullName { get; set; }
        public string ExpertMobileNo { get; set; }

        public BitDelegateCommand GoToMain { get; set; }
        public BitDelegateCommand Call { get; set; }

        public bool IsVisibleBefore { get; set; } = true;
        public bool IsVisibleAfter { get; set; } = false;

        public string Message { get; set; }

        public EvlRequestWaitViewModel(INavigationService navigationService,
            IODataClient odataClient,
            IDeviceService deviceService,
            IODataClient oDataClient)
        {
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

        public async override Task OnNavigatingToAsync(NavigationParameters parameters)
        {
            _evlRequest = parameters.GetValue<EvlRequestDto>("EvlRequestDto");

            EvlRequestExpertDto evlRequestExpert = await _odataClient.For<EvlRequestExpertDto>("EvlRequestExperts")
                .Function("FindEvlRequestExpert")
                .Set(new { evlRequestId = _evlRequest.Id })
                .FindEntryAsync();

            ExpertFullName = evlRequestExpert.ExpName;

            await base.OnNavigatingToAsync(parameters);
        }
    }
}

using Bit.ViewModel;
using Plugin.Geolocator.Abstractions;
using Prism.Navigation;
using Prism.Services;
using Sanaap.Dto;
using Simple.OData.Client;
using System.Linq;

namespace Sanaap.App.ViewModels
{
    public class MyEvlRequestsViewModel : BitViewModelBase
    {
        private readonly IODataClient _odataClient;

        public EvlRequestDto[] MyEvlRequests { get; set; }

        public MyEvlRequestsViewModel(INavigationService navigationService,
            IGeolocator geolocator,
            IODataClient odataClient,
            IPageDialogService pageDialogService)
        {
            _odataClient = odataClient;
        }

        public async override void OnNavigatedTo(NavigationParameters parameters)
        {
            MyEvlRequests = (await _odataClient.For<EvlRequestDto>("EvlRequests")
                    .Function("GetMyEvlRequests")
                    .OrderBy(it => it.ModifiedOn)
                    .FindEntriesAsync())
                    .ToArray();

            base.OnNavigatedTo(parameters);
        }
    }
}
using Bit.ViewModel;
using Plugin.Geolocator.Abstractions;
using Prism.Navigation;
using Prism.Services;
using Sanaap.Dto;
using Simple.OData.Client;
using System.Linq;

namespace Sanaap.App.ViewModels
{
    public class MySosRequestsViewModel : BitViewModelBase
    {
        private readonly IODataClient _odataClient;

        public bool IsBusy { get; set; }

        public SosRequestDto[] MySosRequests { get; set; }

        public MySosRequestsViewModel(INavigationService navigationService,
            IGeolocator geolocator,
            IODataClient odataClient,
            IPageDialogService pageDialogService)
        {
            _odataClient = odataClient;
        }

        public async override void OnNavigatedTo(NavigationParameters parameters)
        {
            IsBusy = true;
            try
            {
                MySosRequests = (await _odataClient.For<SosRequestDto>("SosRequests")
                    .Function("GetMySosRequests")
                    .OrderBy(it => it.ModifiedOn)
                    .FindEntriesAsync())
                    .ToArray();

                base.OnNavigatedTo(parameters);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
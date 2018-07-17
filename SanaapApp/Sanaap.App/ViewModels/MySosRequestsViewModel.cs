using Bit.ViewModel;
using Prism.Navigation;
using Prism.Services;
using Sanaap.Dto;
using Simple.OData.Client;
using System.Linq;
using System.Threading.Tasks;

namespace Sanaap.App.ViewModels
{
    public class MySosRequestsViewModel : BitViewModelBase
    {
        private readonly IODataClient _odataClient;
        private readonly IPageDialogService _pageDialogService;
        private readonly INavigationService _navigationService;

        public SosRequestDto[] MySosRequests { get; set; }

        public MySosRequestsViewModel(
            INavigationService navigationService,
            IODataClient odataClient,
            IPageDialogService pageDialogService)
        {
            _odataClient = odataClient;
            _pageDialogService = pageDialogService;
            _navigationService = navigationService;
        }

        public async override Task OnNavigatedToAsync(NavigationParameters parameters)
        {
            string path = _navigationService.GetNavigationUriPath();

            MySosRequests = (await _odataClient.For<SosRequestDto>("SosRequests")
                .Function("GetMySosRequests")
                .OrderBy(it => it.ModifiedOn)
                .FindEntriesAsync())
                .ToArray();

            await base.OnNavigatedToAsync(parameters);
        }
    }
}

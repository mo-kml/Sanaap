using Bit.ViewModel;
using Prism.Navigation;
using Prism.Services;
using Sanaap.Dto;
using Simple.OData.Client;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms.GoogleMaps;

namespace Sanaap.App.ViewModels
{
    public class SubmitSosRequestViewModel : BitViewModelBase
    {
        public BitDelegateCommand SubmitSosRequest { get; set; }

        public BitDelegateCommand<Map> UpdateCurrentLocation { get; set; }

        public string Description { get; set; }

        public bool IsPositionSelected { get; set; }

        public Location CurrentPosition { get; set; }

        private readonly IODataClient _odataClient;
        private readonly IPageDialogService _pageDialogService;

        public SubmitSosRequestViewModel(INavigationService navigationService,
            IODataClient odataClient,
            IPageDialogService pageDialogService)
        {
            _odataClient = odataClient;
            _pageDialogService = pageDialogService;

            SubmitSosRequest = new BitDelegateCommand(async () =>
            {
                if (await pageDialogService.DisplayAlertAsync("", "مطمئن هستید؟", "بله", "خیر"))
                {
                    SosRequestDto sosReq = new SosRequestDto
                    {
                        SosRequestStatus = Enums.EvlRequestStatus.SabteAvalie,
                        Latitude = CurrentPosition.Latitude,
                        Longitude = CurrentPosition.Longitude,
                        Description = Description
                    };

                    await odataClient.For<SosRequestDto>("SosRequests")
                       .Action("SubmitSosRequest")
                       .Set(new { sosReq })
                       .ExecuteAsync();

                    await pageDialogService.DisplayAlertAsync("", "درخواست شما با موفقیت ارسال شد ، با شما تماس میگیریم", "ممنون");

                    await navigationService.NavigateAsync("/Menu/Nav/Main");
                }
            });

            UpdateCurrentLocation = new BitDelegateCommand<Map>(async (map) =>
            {
                Position centerPosition = map.VisibleRegion.Center;

                CurrentPosition = new Location { Latitude = centerPosition.Latitude, Longitude = centerPosition.Longitude };

                IsPositionSelected = true;
            });
        }

        public async override Task OnNavigatedToAsync(NavigationParameters parameters)
        {
            IsPositionSelected = false;

            CurrentPosition = await GeolocationExtensions.GetLocation();

            await base.OnNavigatedToAsync(parameters);
        }
    }
}

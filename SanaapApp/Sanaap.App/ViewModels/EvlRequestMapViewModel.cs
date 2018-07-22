using Acr.UserDialogs;
using Bit.ViewModel;
using Plugin.Geolocator.Abstractions;
using Prism.Navigation;
using Prism.Services;
using Sanaap.App.Dto;
using Sanaap.Constants;
using Sanaap.Enums;
using System.Threading.Tasks;
using Xamarin.Forms.GoogleMaps;

namespace Sanaap.App.ViewModels
{
    public class EvlRequestMapViewModel : BitViewModelBase
    {
        private EvlRequestDto evlRequestDto;
        private InsuranceType insuranceType;

        public BitDelegateCommand<Map> UpdateLocationAndGotoDetail { get; set; }

        public Plugin.Geolocator.Abstractions.Position CurrentPosition { get; set; }

        private readonly IGeolocator _geolocator;
        private readonly IPageDialogService _pageDialogService;
        private readonly IUserDialogs _userDialogs;

        public EvlRequestMapViewModel(INavigationService navigationService,
            IGeolocator geolocator,
            IPageDialogService pageDialogService,
            IUserDialogs userDialogs)
        {
            _geolocator = geolocator;
            _pageDialogService = pageDialogService;
            _userDialogs = userDialogs;

            UpdateLocationAndGotoDetail = new BitDelegateCommand<Map>(async (map) =>
            {
                Xamarin.Forms.GoogleMaps.Position centerPosition = map.VisibleRegion.Center;

                if (evlRequestDto == null)
                    evlRequestDto = new EvlRequestDto();

                evlRequestDto.Latitude = centerPosition.Latitude;
                evlRequestDto.Longitude = centerPosition.Longitude;
                evlRequestDto.InsuranceTypeEnum = insuranceType;

                await navigationService.NavigateAsync("EvlRequestDetail", new NavigationParameters
                {
                    { "EvlRequestDto" , evlRequestDto }
                });
            });
        }

        public async override Task OnNavigatedToAsync(NavigationParameters parameters)
        {
            using (_userDialogs.Loading(ConstantStrings.Loading))
            {
                if (parameters.GetNavigationMode() == NavigationMode.New)
                {
                    insuranceType = parameters.GetValue<InsuranceType>("InsuranceType"); // Get Parameter

                    if (_geolocator.IsGeolocationAvailable)
                    {
                        CurrentPosition = await _geolocator.GetPositionAsync();
                    }
                }

                if (parameters.GetNavigationMode() == NavigationMode.Back)
                {
                    evlRequestDto = parameters.GetValue<EvlRequestDto>("EvlRequestDto");
                    CurrentPosition = new Plugin.Geolocator.Abstractions.Position
                    {
                        Latitude = evlRequestDto.Latitude,
                        Longitude = evlRequestDto.Longitude
                    };
                }

                await base.OnNavigatedToAsync(parameters);
            }
        }
    }
}

using Acr.UserDialogs;
using Bit.ViewModel;
using Prism.Navigation;
using Prism.Services;
using Sanaap.App.Dto;
using Sanaap.Constants;
using Sanaap.Enums;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms.GoogleMaps;

namespace Sanaap.App.ViewModels
{
    public class EvlRequestMapViewModel : BitViewModelBase
    {
        private EvlRequestDto evlRequestDto;
        private InsuranceType insuranceType;

        public BitDelegateCommand<Map> UpdateLocationAndGotoDetail { get; set; }

        public Location CurrentPosition { get; set; }

        private readonly IPageDialogService _pageDialogService;
        private readonly IUserDialogs _userDialogs;

        private CancellationTokenSource registerCancellationTokenSource;

        public EvlRequestMapViewModel(INavigationService navigationService,
            IPageDialogService pageDialogService,
            IUserDialogs userDialogs)
        {
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
            registerCancellationTokenSource?.Cancel();
            registerCancellationTokenSource = new CancellationTokenSource();
            using (_userDialogs.Loading(ConstantStrings.Loading, cancelText: ConstantStrings.Loading_Cancel, onCancel: registerCancellationTokenSource.Cancel))
            {
                if (parameters.GetNavigationMode() == NavigationMode.New)
                {
                    insuranceType = parameters.GetValue<InsuranceType>("InsuranceType");

                    CurrentPosition = await GeolocationExtensions.GetLocation();
                }

                if (parameters.GetNavigationMode() == NavigationMode.Back)
                {
                    evlRequestDto = parameters.GetValue<EvlRequestDto>("EvlRequestDto");
                    CurrentPosition = new Location
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

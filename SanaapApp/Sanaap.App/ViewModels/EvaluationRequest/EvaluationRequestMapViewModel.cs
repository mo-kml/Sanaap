using Acr.UserDialogs;
using Bit.ViewModel;
using Prism.Navigation;
using Sanaap.App.ItemSources;
using Sanaap.App.Views.EvaluationRequest;
using Sanaap.Constants;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms.GoogleMaps;

namespace Sanaap.App.ViewModels.EvaluationRequest
{
    public class EvaluationRequestMapViewModel : BitViewModelBase
    {
        public BitDelegateCommand<Xamarin.Forms.GoogleMaps.Map> UpdateLocation { get; set; }

        public Location CurrentPosition { get; set; }

        public EvlRequestItemSource Request { get; set; }

        public BitDelegateCommand GoBack { get; set; }

        private CancellationTokenSource registerCancellationTokenSource;

        private readonly IUserDialogs _userDialogs;
        public EvaluationRequestMapViewModel(IUserDialogs userDialogs)
        {
            _userDialogs = userDialogs;

            UpdateLocation = new BitDelegateCommand<Xamarin.Forms.GoogleMaps.Map>(async (map) =>
            {
                Position centerPosition = map.VisibleRegion.Center;

                Request.Latitude = centerPosition.Latitude;
                Request.Longitude = centerPosition.Longitude;

                //Geocoder geocoder = new Geocoder();
                //Request.Address = (await geocoder.GetAddressesForPositionAsync(centerPosition)).FirstOrDefault();

                await NavigationService.NavigateAsync(nameof(EvaluationRequestFilesView), new NavigationParameters
                {
                    {nameof(Request),Request }
                });
            });

            GoBack = new BitDelegateCommand(async () =>
            {
                await NavigationService.GoBackAsync();
            });
        }

        public override async Task OnNavigatedToAsync(INavigationParameters parameters)
        {
            registerCancellationTokenSource?.Cancel();
            registerCancellationTokenSource = new CancellationTokenSource();
            using (_userDialogs.Loading(ConstantStrings.Loading, cancelText: ConstantStrings.Loading_Cancel, onCancel: registerCancellationTokenSource.Cancel))
            {
                Request = parameters.GetValue<EvlRequestItemSource>(nameof(Request));

                if (Request.Latitude == 0)
                {
                    CurrentPosition = await Geolocation.GetLocationAsync();
                }
                else
                {
                    CurrentPosition = new Location
                    {
                        Latitude = Request.Latitude,
                        Longitude = Request.Longitude
                    };
                }

                await base.OnNavigatedToAsync(parameters);
            }
        }

        public override Task OnNavigatedFromAsync(INavigationParameters parameters)
        {
            parameters.Add(nameof(Request), Request);
            return base.OnNavigatedFromAsync(parameters);
        }
    }
}

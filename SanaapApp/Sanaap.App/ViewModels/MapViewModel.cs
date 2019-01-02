using Acr.UserDialogs;
using Bit.ViewModel;
using Prism.Navigation;
using Prism.Services;
using Sanaap.Constants;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms.GoogleMaps;

namespace Sanaap.App.ViewModels
{
    public class MapViewModel : BitViewModelBase
    {
        public BitDelegateCommand<Map> UpdateLocation { get; set; }

        public Location CurrentPosition { get; set; }

        public NavigationParameters Parameters { get; set; }

        private CancellationTokenSource registerCancellationTokenSource;

        public string NextPage { get; set; }

        private readonly IPageDialogService _pageDialogService;
        private readonly IUserDialogs _userDialogs;
        public MapViewModel(INavigationService navigationService,
            IPageDialogService pageDialogService,
            IUserDialogs userDialogs)
        {
            _pageDialogService = pageDialogService;
            _userDialogs = userDialogs;

            UpdateLocation = new BitDelegateCommand<Map>(async (map) =>
             {
                 Position centerPosition = map.VisibleRegion.Center;

                 Parameters.Add(nameof(Position), centerPosition);

                 await navigationService.NavigateAsync(NextPage, Parameters);
             });
        }

        public override async Task OnNavigatedToAsync(NavigationParameters parameters)
        {
            registerCancellationTokenSource?.Cancel();
            registerCancellationTokenSource = new CancellationTokenSource();
            using (_userDialogs.Loading(ConstantStrings.Loading, cancelText: ConstantStrings.Loading_Cancel, onCancel: registerCancellationTokenSource.Cancel))
            {
                if (parameters.GetNavigationMode() == NavigationMode.New)
                {
                    Parameters = parameters;

                    if (parameters.TryGetValue(nameof(NextPage), out string nextPage))
                    {
                        NextPage = nextPage;
                    }
                    else
                    {
                        throw new ArgumentNullException(nameof(NextPage));
                    }

                    CurrentPosition = await GeolocationExtensions.GetLocation();
                }

                if (parameters.GetNavigationMode() == NavigationMode.Back)
                {
                    if (!parameters.TryGetValue(nameof(Position), out Position position))
                    {
                        throw new ArgumentNullException(nameof(Position));
                    }

                    CurrentPosition = new Xamarin.Essentials.Location
                    {
                        Latitude = position.Latitude,
                        Longitude = position.Longitude
                    };
                }

                await base.OnNavigatedToAsync(parameters);
            }
        }

        public override Task OnNavigatedFromAsync(NavigationParameters parameters)
        {
            parameters = Parameters;
            return base.OnNavigatedFromAsync(parameters);
        }
    }
}

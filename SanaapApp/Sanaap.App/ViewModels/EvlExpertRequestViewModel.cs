using Bit.ViewModel;
using Microsoft.AppCenter.Crashes;
using Plugin.Connectivity.Abstractions;
using Plugin.Geolocator.Abstractions;
using Prism.Navigation;
using Prism.Services;
using Sanaap.Constants;
using System;
using Xamarin.Forms.GoogleMaps;

namespace Sanaap.App.ViewModels
{
    public class EvlExpertRequestViewModel : BitViewModelBase
    {
        public BitDelegateCommand GoToNextPage { get; set; }

        public BitDelegateCommand<Map> UpdateCurrentLocation { get; set; }

        public Plugin.Geolocator.Abstractions.Position CurrentPosition { get; set; }
        private readonly IGeolocator _geolocator;
        private readonly IPageDialogService _pageDialogService;
        private readonly IConnectivity _connectivity;

        public EvlExpertRequestViewModel(INavigationService navigationService,
            IGeolocator geolocator,
            IPageDialogService pageDialogService,
            IConnectivity connectivity)
        {
            _geolocator = geolocator;
            _pageDialogService = pageDialogService;
            _connectivity = connectivity;

            GoToNextPage = new BitDelegateCommand(async () =>
            {
                try
                {
                    if (connectivity.IsConnected == false)
                    {
                        await pageDialogService.DisplayAlertAsync(string.Empty, ErrorMessages.UnreachableNetwork, ErrorMessages.Ok);
                        return;
                    }
                    var navigationParameter = new NavigationParameters();
                    navigationParameter.Add("Position", CurrentPosition);

                    await navigationService.NavigateAsync("EvlExpertRequestDetail", navigationParameter);
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                    await _pageDialogService.DisplayAlertAsync(string.Empty, ErrorMessages.UnknownError, ErrorMessages.Ok);

                }
            }, () => CurrentPosition != null);
            GoToNextPage.ObservesProperty(() => CurrentPosition);

            UpdateCurrentLocation = new BitDelegateCommand<Map>((map) =>
            {
                Xamarin.Forms.GoogleMaps.Position centerPosition = map.VisibleRegion.Center;

                CurrentPosition = new Plugin.Geolocator.Abstractions.Position { Latitude = centerPosition.Latitude, Longitude = centerPosition.Longitude };
            });
        }

        public async override void OnNavigatedTo(NavigationParameters parameters)
        {
            try
            {
                if (_connectivity.IsConnected == false)
                {
                    await _pageDialogService.DisplayAlertAsync(string.Empty, ErrorMessages.UnknownError, ErrorMessages.Ok);
                    return;
                }

                if (_geolocator.IsGeolocationAvailable)
                {
                    try
                    {
                        CurrentPosition = await _geolocator.GetPositionAsync();
                    }
                    catch (Exception ex) { throw ex; }
                }

                base.OnNavigatedTo(parameters);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                await _pageDialogService.DisplayAlertAsync(string.Empty, ErrorMessages.UnknownError, ErrorMessages.Ok);

            }
        }
    }
}

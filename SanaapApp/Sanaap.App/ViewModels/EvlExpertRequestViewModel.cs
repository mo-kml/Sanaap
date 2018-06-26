using Acr.UserDialogs;
using Bit.ViewModel;
using Microsoft.AppCenter.Crashes;
using Plugin.Connectivity.Abstractions;
using Plugin.Geolocator.Abstractions;
using Prism.Navigation;
using Prism.Services;
using Sanaap.App.Dto;
using Sanaap.Constants;
using System;
using Xamarin.Forms.GoogleMaps;

namespace Sanaap.App.ViewModels
{
    public class EvlExpertRequestViewModel : BitViewModelBase
    {
        private EvlExpertRequestDto evlExpertRequestDto;

        public BitDelegateCommand GoToNextPage { get; set; }

        public BitDelegateCommand<Map> UpdateCurrentLocation { get; set; }

        public bool IsPositionSelected { get; set; } = false;
        public Plugin.Geolocator.Abstractions.Position CurrentPosition { get; set; }
        private readonly IGeolocator _geolocator;
        private readonly IPageDialogService _pageDialogService;
        private readonly IConnectivity _connectivity;
        private readonly IUserDialogs _userDialogs;
        public EvlExpertRequestViewModel(INavigationService navigationService,
            IGeolocator geolocator,
            IPageDialogService pageDialogService,
            IConnectivity connectivity,
            IUserDialogs userDialogs)
        {
            _geolocator = geolocator;
            _pageDialogService = pageDialogService;
            _connectivity = connectivity;
            _userDialogs = userDialogs;

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

                    if (evlExpertRequestDto == null)
                        evlExpertRequestDto = new EvlExpertRequestDto();

                    evlExpertRequestDto.Latitude = CurrentPosition.Latitude;
                    evlExpertRequestDto.Longitude = CurrentPosition.Longitude;

                    navigationParameter.Add("EvlExpertRequestDto", evlExpertRequestDto);

                    await navigationService.NavigateAsync("EvlExpertRequestDetail", navigationParameter);
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                    await _pageDialogService.DisplayAlertAsync(string.Empty, ErrorMessages.UnknownError, ErrorMessages.Ok);

                }
            }, () => IsPositionSelected == true);
            GoToNextPage.ObservesProperty(() => IsPositionSelected);

            UpdateCurrentLocation = new BitDelegateCommand<Map>((map) =>
            {
                Xamarin.Forms.GoogleMaps.Position centerPosition = map.VisibleRegion.Center;

                CurrentPosition = new Plugin.Geolocator.Abstractions.Position
                {
                    Latitude = centerPosition.Latitude,
                    Longitude = centerPosition.Longitude
                };

                IsPositionSelected = true;
            });
        }

        public async override void OnNavigatedTo(NavigationParameters parameters)
        {
            IsPositionSelected = false;

            if (parameters.TryGetValue("EvlExpertRequestDto", out evlExpertRequestDto))
            {
                CurrentPosition = new Plugin.Geolocator.Abstractions.Position
                {
                    Latitude = evlExpertRequestDto.Latitude,
                    Longitude = evlExpertRequestDto.Longitude
                };
            }
            else
            {
                try
                {
                    using (_userDialogs.Loading(ConstantStrings.Loading))
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

                                //CurrentPosition = null;
                            }
                            catch (Exception ex) { throw ex; }
                        }
                    }

                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                    await _pageDialogService.DisplayAlertAsync(string.Empty, ErrorMessages.UnknownError, ErrorMessages.Ok);

                }
            }
            base.OnNavigatedTo(parameters);
        }
    }
}

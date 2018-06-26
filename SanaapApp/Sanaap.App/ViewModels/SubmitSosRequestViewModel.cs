using Bit.ViewModel;
using Microsoft.AppCenter.Crashes;
using Plugin.Connectivity.Abstractions;
using Plugin.Geolocator.Abstractions;
using Prism.Navigation;
using Prism.Services;
using Sanaap.Dto;
using Simple.OData.Client;
using System;
using Xamarin.Forms.GoogleMaps;
using static Sanaap.Enums.Enums;

namespace Sanaap.App.ViewModels
{
    public class SubmitSosRequestViewModel : BitViewModelBase
    {
        public BitDelegateCommand SubmitSosRequest { get; set; }

        public BitDelegateCommand<Map> UpdateCurrentLocation { get; set; }

        public string Description { get; set; }

        public bool IsPositionSelected { get; set; }

        public Plugin.Geolocator.Abstractions.Position CurrentPosition { get; set; }
        private readonly IGeolocator _geolocator;
        private readonly IODataClient _odataClient;
        private readonly IPageDialogService _pageDialogService;
        private readonly IConnectivity _connectivity;

        public SubmitSosRequestViewModel(INavigationService navigationService,
            IGeolocator geolocator,
            IODataClient odataClient,
            IPageDialogService pageDialogService,
            IConnectivity connectivity)
        {
            _geolocator = geolocator;
            _odataClient = odataClient;
            _pageDialogService = pageDialogService;
            _connectivity = connectivity;

            SubmitSosRequest = new BitDelegateCommand(async () =>
            {
                try
                {
                    if (connectivity.IsConnected == false)
                    {
                        await pageDialogService.DisplayAlertAsync("", "ارتباط با اینترنت برقرار نیست", "باشه");
                        return;
                    }

                    bool confirmed = await pageDialogService.DisplayAlertAsync("", "مطمئن هستید؟", "بله", "خیر");

                    if (confirmed)
                    {
                        SosRequestDto sosReq = new SosRequestDto
                        {
                            SosRequestStatus = EnumSosRequestStatus.SabteAvalie,
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
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                    await pageDialogService.DisplayAlertAsync("", "خطای نامشخص", "باشه");
                }
            });

            UpdateCurrentLocation = new BitDelegateCommand<Map>((map) =>
              {
                  Xamarin.Forms.GoogleMaps.Position centerPosition = map.VisibleRegion.Center;

                  CurrentPosition = new Plugin.Geolocator.Abstractions.Position { Latitude = centerPosition.Latitude, Longitude = centerPosition.Longitude };

                  IsPositionSelected = true;
              });
        }

        public async override void OnNavigatedTo(NavigationParameters parameters)
        {
            IsPositionSelected = false;

            try
            {
                if (_connectivity.IsConnected == false)
                {
                    await _pageDialogService.DisplayAlertAsync("", "ارتباط با اینترنت برقرار نیست", "باشه");
                    return;
                }

                //SosRequestStatus = (await _odataClient.For<SosRequestStatusDto>("SosRequestStatuses")
                //.OrderBy(it => it.Code)
                //.FindEntriesAsync())
                //.ToArray().FirstOrDefault();

                if (_geolocator.IsGeolocationAvailable)
                {
                    try
                    {
                        CurrentPosition = await _geolocator.GetPositionAsync();
                    }
                    catch { }
                }

                base.OnNavigatedTo(parameters);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                await _pageDialogService.DisplayAlertAsync("", "خطای نامشخص", "باشه");
            }
        }
    }
}

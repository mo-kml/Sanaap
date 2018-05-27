﻿using Bit.ViewModel;
using Microsoft.AppCenter.Crashes;
using Plugin.Connectivity.Abstractions;
using Plugin.Geolocator.Abstractions;
using Prism.Navigation;
using Prism.Services;
using Sanaap.Dto;
using Simple.OData.Client;
using System;
using System.Linq;

namespace Sanaap.App.ViewModels
{
    public class SubmitSosRequestViewModel : BitViewModelBase
    {
        public BitDelegateCommand SubmitSosRequest { get; set; }

        public SosRequestStatusDto SosRequestStatus { get; set; }

        public SosRequestStatusDto SelectedSosRequestStatus { get; set; }

        public string Description { get; set; }

        public bool IsBusy { get; set; } = false;

        public bool CanSend { get; set; } = false;

        public Position CurrentPosition { get; set; } = new Position(35, 51);
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
                IsBusy = true;

                try
                {
                    if (connectivity.IsConnected == false)
                    {
                        await pageDialogService.DisplayAlertAsync("", "ارتباط با اینترنت برقرار نیست", "باشه");
                        return;
                    }

                    SosRequestDto sosReq = new SosRequestDto
                    {
                        SosRequestStatusId = SosRequestStatus.Id,
                        Latitude = CurrentPosition.Latitude,
                        Longitude = CurrentPosition.Longitude,
                        Description = Description
                    };

                    bool confirmed = await pageDialogService.DisplayAlertAsync("", "مطمئن هستید؟", "بله", "خیر");

                    if (confirmed)
                    {
                        await odataClient.For<SosRequestDto>("SosRequests")
                           .Action("SubmitSosRequest")
                           .Set(new { sosReq })
                           .ExecuteAsync();
                        await pageDialogService.DisplayAlertAsync("", "درخواست شما با موفقیت ارسال شد", "ممنون");
                        await navigationService.NavigateAsync("Main");
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                    await pageDialogService.DisplayAlertAsync("", "خطای نامشخص", "باشه");
                }
                finally
                {
                    IsBusy = false;
                }
            });
        }

        public async override void OnNavigatedTo(NavigationParameters parameters)
        {
            try
            {
                if (_connectivity.IsConnected == false)
                {
                    await _pageDialogService.DisplayAlertAsync("", "ارتباط با اینترنت برقرار نیست", "باشه");
                    return;
                }

                IsBusy = true; CanSend = false;

                SosRequestStatus = (await _odataClient.For<SosRequestStatusDto>("SosRequestStatuses")
                    .OrderBy(it => it.Code)
                    .FindEntriesAsync())
                    .ToArray().FirstOrDefault();

                if (_geolocator.IsGeolocationAvailable)
                {
                    try
                    {
                        CurrentPosition = await _geolocator.GetPositionAsync();
                    }
                    finally
                    {
                        CanSend = true;
                        IsBusy = false;
                    }
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
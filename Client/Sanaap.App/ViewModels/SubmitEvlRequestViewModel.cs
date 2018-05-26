using Bit.ViewModel;
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

        public virtual Position CurrentPosition { get; set; } = new Position(35, 51);
        private readonly IGeolocator _geolocator;
        private readonly IODataClient _odataClient;

        public SubmitSosRequestViewModel(INavigationService navigationService, IGeolocator geolocator, IODataClient odataClient, IPageDialogService pageDialogService)
        {
            _geolocator = geolocator;
            _odataClient = odataClient;

            SubmitSosRequest = new BitDelegateCommand(async () =>
            {
                IsBusy = true;

                try
                {
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
                        await navigationService.NavigateAsync("Menu");
                    }
                }
                catch (Exception ex)
                {
                    await pageDialogService.DisplayAlertAsync("", ex.Message, "باشه");
                    return;
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
                SosRequestStatus = (await _odataClient.For<SosRequestStatusDto>("SosRequestStatuses")
                    .OrderBy(it => it.Code)
                    .FindEntriesAsync())
                    .ToArray().FirstOrDefault();

                if (_geolocator.IsGeolocationAvailable)
                {
                    IsBusy = true; CanSend = false;
                    try
                    {
                        CurrentPosition = await _geolocator.GetPositionAsync();
                        CanSend = true;
                    }
                    finally
                    {
                        IsBusy = false;
                    }
                }

                base.OnNavigatedTo(parameters);
            }
            catch (Exception ex)
            {

            }
        }
    }
}
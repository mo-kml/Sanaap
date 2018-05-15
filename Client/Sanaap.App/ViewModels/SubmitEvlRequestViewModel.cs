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
    public class SubmitEvlRequestViewModel : BitViewModelBase
    {
        public BitDelegateCommand SubmitEvlRequest { get; set; }

        public InsuranceTypeDto[] InsuranceTypes { get; set; }

        public InsuranceTypeDto SelectedInsuranceType { get; set; }

        public bool IsBusy { get; set; } = false;

        public bool CanSend { get; set; } = false;

        private readonly IGeolocator _geolocator;
        private readonly IODataClient _odataClient;

        public SubmitEvlRequestViewModel(INavigationService navigationService, IGeolocator geolocator, IODataClient odataClient
            , IPageDialogService pageDialogService)
        {
            _geolocator = geolocator;
            _odataClient = odataClient;

            try
            {
                SubmitEvlRequest = new BitDelegateCommand(async () =>
                {
                    EvlRequestDto evlReq = new EvlRequestDto
                    {
                        InsuranceTypeId = SelectedInsuranceType.Id,
                        Latitude = CurrentPosition.Latitude,
                        Longitude = CurrentPosition.Longitude
                    };
                    bool confirmed = await pageDialogService.DisplayAlertAsync("", "مطمئن هستید؟", "بله", "خیر");
                    if (confirmed)
                    {
                        IsBusy = true;
                        await odataClient.For<EvlRequestDto>("EvlRequests")
                           .Action("SubmitEvlRequest")
                           .Set(new { evlReq })
                           .ExecuteAsync();
                        IsBusy = false;
                        await pageDialogService.DisplayAlertAsync("", "درخواست شما با موفقیت ارسال شد", "ممنون");
                        await navigationService.NavigateAsync("Main");
                    }
                    else return;
                });

                SubmitEvlRequest.ObservesProperty(() => CurrentPosition);
                SubmitEvlRequest.ObservesProperty(() => SelectedInsuranceType);
            }
            catch (Exception ex)
            {
                pageDialogService.DisplayAlertAsync("", ex.Message, "باشه");
                return;
            }
        }

        public virtual Position CurrentPosition { get; set; } = new Position(35, 51);


        public async override void OnNavigatedTo(NavigationParameters parameters)
        {
            try
            {
                InsuranceTypes = (await _odataClient.For<InsuranceTypeDto>("InsuranceTypes")
                    .OrderBy(it => it.Code)
                    .FindEntriesAsync())
                    .ToArray();

                SelectedInsuranceType = InsuranceTypes.First();

                if (_geolocator.IsGeolocationAvailable)
                {
                    IsBusy = true; CanSend = false;
                    CurrentPosition = await _geolocator.GetPositionAsync();
                    IsBusy = false; CanSend = true;
                }

                base.OnNavigatedTo(parameters);
            }
            catch (Exception ex)
            {
                return;
            }
        }
    }
}

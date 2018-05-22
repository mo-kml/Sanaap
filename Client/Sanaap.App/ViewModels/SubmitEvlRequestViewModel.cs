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

        public CarTypeDto[] CarTypes { get; set; }

        public InsuranceTypeDto SelectedInsuranceType { get; set; }

        public CarTypeDto SelectedCarType { get; set; }

        public bool IsBusy { get; set; } = false;

        public bool CanSend { get; set; } = false;

        public virtual Position CurrentPosition { get; set; } = new Position(35, 51);
        private readonly IGeolocator _geolocator;
        private readonly IODataClient _odataClient;

        public SubmitEvlRequestViewModel(INavigationService navigationService, IGeolocator geolocator, IODataClient odataClient, IPageDialogService pageDialogService)
        {
            _geolocator = geolocator;
            _odataClient = odataClient;

            SubmitEvlRequest = new BitDelegateCommand(async () =>
            {
                IsBusy = true;

                try
                {
                    EvlRequestDto evlReq = new EvlRequestDto
                    {
                        InsuranceTypeId = SelectedInsuranceType.Id,
                        CarTypeId = SelectedCarType.Id,
                        Latitude = CurrentPosition.Latitude,
                        Longitude = CurrentPosition.Longitude
                    };
                    bool confirmed = await pageDialogService.DisplayAlertAsync("", "مطمئن هستید؟", "بله", "خیر");
                    if (confirmed)
                    {
                        await odataClient.For<EvlRequestDto>("EvlRequests")
                           .Action("SubmitEvlRequest")
                           .Set(new { evlReq })
                           .ExecuteAsync();
                        await pageDialogService.DisplayAlertAsync("", "درخواست شما با موفقیت ارسال شد", "ممنون");
                        await navigationService.NavigateAsync("Main");
                    }
                }
                catch(Exception ex)
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
                InsuranceTypes = (await _odataClient.For<InsuranceTypeDto>("InsuranceTypes")
                    .OrderBy(it => it.Code)
                    .FindEntriesAsync())
                    .ToArray();

                CarTypes = (await _odataClient.For<CarTypeDto>("CarTypes")
                    .OrderBy(it => it.Code)
                    .FindEntriesAsync())
                    .ToArray();

                SelectedInsuranceType = InsuranceTypes.First();
                SelectedCarType = CarTypes.First();

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
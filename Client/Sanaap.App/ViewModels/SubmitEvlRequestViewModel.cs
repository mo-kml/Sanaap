using Bit.ViewModel;
using Plugin.Geolocator.Abstractions;
using Prism.Navigation;
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

        private readonly IGeolocator _geolocator;
        private readonly IODataClient _odataClient;

        public SubmitEvlRequestViewModel(IGeolocator geolocator, IODataClient odataClient)
        {
            _geolocator = geolocator;
            _odataClient = odataClient;

            SubmitEvlRequest = new BitDelegateCommand(async () =>
            {
                EvlRequestDto evlReq = new EvlRequestDto
                {
                    InsuranceTypeId = SelectedInsuranceType.Id,
                    Latitude = CurrentPosition.Latitude,
                    Longitude = CurrentPosition.Longitude
                };

                await odataClient.For<EvlRequestDto>("EvlRequests")
                    .Action("SubmitEvlRequest")
                    .Set(new { evlReq })
                    .ExecuteAsync();
            }, () => CurrentPosition != null && SelectedInsuranceType != null);

            SubmitEvlRequest.ObservesProperty(() => CurrentPosition);
            SubmitEvlRequest.ObservesProperty(() => SelectedInsuranceType);
        }

        public virtual Position CurrentPosition { get; set; }

        public async override void OnNavigatedTo(NavigationParameters parameters)
        {
            InsuranceTypes = (await _odataClient.For<InsuranceTypeDto>("InsuranceTypes")
                .OrderBy(it => it.Code)
                .FindEntriesAsync())
                .ToArray();

            SelectedInsuranceType = InsuranceTypes.First();

            if (_geolocator.IsGeolocationAvailable)
            {
                CurrentPosition = await _geolocator.GetPositionAsync();
            }

            base.OnNavigatedTo(parameters);
        }
    }
}

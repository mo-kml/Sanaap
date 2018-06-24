using Bit.ViewModel;
using Prism.Navigation;
using Sanaap.App.Dto;
using Sanaap.Service.Implementations;
using System;
using System.Linq;
using static Sanaap.Enums.Enums;

namespace Sanaap.App.ViewModels
{
    public class EvlExpertRequestDetailViewModel : BitViewModelBase
    {
        Plugin.Geolocator.Abstractions.Position position;

        public string[] InsuranceTypeEnums { get; set; }

        public string SelectedInsuranceTypeEnum { get; set; }

        public InsuranceTypeEnum insuranceType { get; set; }

        public CompanyDto[] Companies { get; set; }

        public CompanyDto SelectedCompany { get; set; }

        public VehicleKindDto[] VehicleKinds { get; set; }

        public VehicleKindDto SelectedVehicleKind { get; set; }

        public DateTime AccidentDate { get; set; } = DateTime.Now;

        public string InsuranceNumber { get; set; }

        public string OwnerFullName { get; set; }

        public string OwnerMobileNumber { get; set; }

        public string Description { get; set; }

        public BitDelegateCommand GoToNextPage { get; set; }


        public EvlExpertRequestDetailViewModel(INavigationService navigationService)
        {
            GoToNextPage = new BitDelegateCommand(() =>
            {
                EvlExpertRequestDto evlExpertRequestDto = new EvlExpertRequestDto
                {
                    Description = Description,
                    OwnerFullName = OwnerFullName,
                    OwnerMobileNumber = OwnerMobileNumber,
                    Latitude = position.Latitude,
                    Longitude = position.Longitude,
                    AccidentDate = DateTimeOffset.Now,
                    CompanyId = SelectedCompany.Id,
                    VehicleKindId = SelectedVehicleKind.Id,
                    InsuranceNumber = InsuranceNumber,
                    InsuranceTypeEnum = (InsuranceTypeEnum)Array.IndexOf(InsuranceTypeEnums, SelectedInsuranceTypeEnum),
                };

                navigationService.NavigateAsync("EvlExpertRequestFiles");

            }, () => SelectedCompany != null && SelectedVehicleKind != null);
            GoToNextPage.ObservesProperty(() => SelectedCompany);
            GoToNextPage.ObservesProperty(() => SelectedVehicleKind);
        }

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            if (!parameters.TryGetValue("Position", out position))
                throw new ArgumentNullException("Position Argumet is Null");

            SyncData();

            base.OnNavigatedTo(parameters);
        }

        public void SyncData()
        {
            Companies = new CompanyDto[] { new CompanyDto { Id = 1, Name = "دانا" }, new CompanyDto { Id = 2, Name = "ایران" }, new CompanyDto { Id = 3, Name = "پارسیان" } };
            SelectedCompany = Companies.First();

            VehicleKinds = new VehicleKindDto[] { new VehicleKindDto { Id = 1, Name = "پژو 405" }, new VehicleKindDto { Id = 2, Name = "پژو 208" }, new VehicleKindDto { Id = 3, Name = "پژو 206" } };
            SelectedVehicleKind = VehicleKinds.First();

            InsuranceTypeEnums = EnumHelper<InsuranceTypeEnum>.GetDisplayValues(insuranceType).ToArray();
            SelectedInsuranceTypeEnum = InsuranceTypeEnums.First();
        }
    }
}

using Acr.UserDialogs;
using Bit.ViewModel;
using Bit.ViewModel.Contracts;
using Newtonsoft.Json;
using Prism.Navigation;
using Prism.Services;
using Sanaap.App.Dto;
using Sanaap.Constants;
using Sanaap.Service.Implementations;
using System;
using System.Linq;
using System.Net.Http;
using static Sanaap.Enums.Enums;

namespace Sanaap.App.ViewModels
{
    public class EvlExpertRequestDetailViewModel : BitViewModelBase
    {
        private readonly HttpClient _httpClient;
        private readonly IClientAppProfile _clientAppProfile;
        private readonly IUserDialogs _userDialogs;

        private EvlExpertRequestDto evlExpertRequestDto;
        public string[] InsuranceTypeEnums { get; set; }
        public string SelectedInsuranceTypeEnum { get; set; }
        public InsuranceTypeEnum insuranceType { get; set; }
        public CompanyDto[] Companies { get; set; }
        public CompanyDto SelectedCompany { get; set; }
        public VehicleKindDto[] VehicleKinds { get; set; }
        public VehicleKindDto SelectedVehicleKind { get; set; }
        public string AccidentDate { get; set; } = Helpers.Helpers.ConvertDateToShamsi(DateTimeOffset.Now);
        public string InsuranceNumber { get; set; }
        public string OwnerFullName { get; set; }
        public string OwnerMobileNumber { get; set; }
        public string Description { get; set; }
        public BitDelegateCommand GoToNextPage { get; set; }

        public EvlExpertRequestDetailViewModel(INavigationService navigationService, IPageDialogService pageDialogService, HttpClient httpClient, IClientAppProfile clientAppProfile, IUserDialogs userDialogs)
        {
            _httpClient = httpClient;
            _clientAppProfile = clientAppProfile;
            _userDialogs = userDialogs;

            //SyncData();

            GoToNextPage = new BitDelegateCommand(async () =>
            {
                if (!Helpers.Helpers.IsShamsiDateValid(AccidentDate))
                {
                    await pageDialogService.DisplayAlertAsync(ErrorMessages.Error, ErrorMessages.IncorrectDateFormat, ErrorMessages.Ok);
                    return;
                }

                using (userDialogs.Loading(ConstantStrings.Loading))
                {
                    evlExpertRequestDto.Description = Description;
                    evlExpertRequestDto.OwnerFullName = OwnerFullName;
                    evlExpertRequestDto.OwnerMobileNumber = OwnerMobileNumber;
                    evlExpertRequestDto.AccidentDate = Helpers.Helpers.ConvertShamsiToMiladi(AccidentDate);
                    evlExpertRequestDto.CompanyId = SelectedCompany.Id;
                    evlExpertRequestDto.VehicleKindId = SelectedVehicleKind.Id;
                    evlExpertRequestDto.InsuranceNumber = InsuranceNumber;
                    evlExpertRequestDto.InsuranceTypeEnum = (InsuranceTypeEnum)Array.IndexOf(InsuranceTypeEnums, SelectedInsuranceTypeEnum);

                    var navigationParameters = new NavigationParameters();
                    navigationParameters.Add("EvlExpertRequestDto", evlExpertRequestDto);

                    await navigationService.NavigateAsync("EvlExpertRequestFiles", navigationParameters);
                }
            }, () => SelectedCompany != null && SelectedVehicleKind != null);
            GoToNextPage.ObservesProperty(() => SelectedCompany);
            GoToNextPage.ObservesProperty(() => SelectedVehicleKind);
        }

        public override void OnNavigatedFrom(NavigationParameters parameters)
        {
            parameters.Add("EvlExpertRequestDto", evlExpertRequestDto);

            base.OnNavigatedFrom(parameters);
        }

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            SyncData();

            parameters.TryGetValue("EvlExpertRequestDto", out evlExpertRequestDto);

            if (evlExpertRequestDto.CompanyId != 0)
            {
                Description = evlExpertRequestDto.Description;
                OwnerFullName = evlExpertRequestDto.OwnerFullName;
                OwnerMobileNumber = evlExpertRequestDto.OwnerMobileNumber;
                AccidentDate = Helpers.Helpers.ConvertDateToShamsi(evlExpertRequestDto.AccidentDate);
                SelectedCompany = Companies.FirstOrDefault(c => c.Id == evlExpertRequestDto.CompanyId);
                SelectedVehicleKind = VehicleKinds.FirstOrDefault(v => v.Id == evlExpertRequestDto.VehicleKindId);
                InsuranceNumber = evlExpertRequestDto.InsuranceNumber;
                SelectedInsuranceTypeEnum = evlExpertRequestDto.InsuranceTypeEnum.ToString();
            }

            base.OnNavigatedTo(parameters);
        }

        public async void SyncData()
        {
            using (_userDialogs.Loading(ConstantStrings.Loading))
            {
                _httpClient.BaseAddress = new Uri($"{_clientAppProfile.HostUri}");

                Companies = JsonConvert.DeserializeObject<CompanyDto[]>(await _httpClient.GetStringAsync(_httpClient.BaseAddress + "api/Companies/GetAll"));
                SelectedCompany = Companies.First();

                VehicleKinds = JsonConvert.DeserializeObject<VehicleKindDto[]>(await _httpClient.GetStringAsync(_httpClient.BaseAddress + "api/VehicleKinds/GetAll"));
                SelectedVehicleKind = VehicleKinds.First();

                InsuranceTypeEnums = EnumHelper<InsuranceTypeEnum>.GetDisplayValues(insuranceType).ToArray();
                SelectedInsuranceTypeEnum = InsuranceTypeEnums.First();
            }
        }
    }
}

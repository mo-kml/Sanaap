using Acr.UserDialogs;
using Bit.ViewModel;
using Bit.ViewModel.Contracts;
using Prism.Navigation;
using Prism.Services;
using Sanaap.App.Dto;
using Sanaap.Constants;
using Sanaap.Enums;
using Sanaap.Service.Contracts;
using Simple.OData.Client;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Sanaap.App.ViewModels
{
    public class EvlRequestDetailViewModel : BitViewModelBase
    {
        private readonly IUserDialogs _userDialogs;
        private readonly IPageDialogService _pageDialogService;
        private readonly IODataClient _odataClient;
        private readonly IDateTimeUtils _dateTimeUtils;
        private EvlRequestDto _evlRequestDto;
        private InsuranceType insuranceType;

        public CompanyDto[] Companies { get; set; }
        public CompanyDto SelectedCompany { get; set; }
        public VehicleKindDto[] VehicleKinds { get; set; }
        public VehicleKindDto SelectedVehicleKind { get; set; }
        public string[] InsuranceTypeEnums { get; set; }
        public string AccidentDate { get; set; }
        public string InsuranceNumber { get; set; }
        public string SelectedInsuranceTypeEnum { get; set; }
        public string OwnerFullName { get; set; }
        public string OwnerMobileNumber { get; set; }
        public string Description { get; set; }
        public BitDelegateCommand GoToNextPage { get; set; }

        public EvlRequestDetailViewModel(INavigationService navigationService
            , IPageDialogService pageDialogService, HttpClient httpClient
            , IClientAppProfile clientAppProfile, IUserDialogs userDialogs
            , IODataClient oDataClient,
            IDateTimeUtils dateTimeUtils,
            IDateTimeProvider dateTimeProvider)
        {
            _userDialogs = userDialogs;
            _pageDialogService = pageDialogService;
            _odataClient = oDataClient;
            _dateTimeUtils = dateTimeUtils;

            AccidentDate = _dateTimeUtils.ConvertDateToShamsi(dateTimeProvider.GetCurrentUtcDateTime());

            GoToNextPage = new BitDelegateCommand(async () =>
            {
                if (!_dateTimeUtils.IsValidShamsiDate(AccidentDate))
                {
                    await pageDialogService.DisplayAlertAsync(ErrorMessages.Error, ErrorMessages.IncorrectDateFormat, ErrorMessages.Ok);
                    return;
                }

                _evlRequestDto.Description = Description;
                _evlRequestDto.OwnerFullName = OwnerFullName;
                _evlRequestDto.OwnerMobileNumber = OwnerMobileNumber;
                _evlRequestDto.AccidentDate = _dateTimeUtils.ConvertShamsiToMiladi(AccidentDate);
                _evlRequestDto.CompanyId = SelectedCompany.Id;
                _evlRequestDto.VehicleKindId = SelectedVehicleKind.Id;
                _evlRequestDto.InsuranceNumber = InsuranceNumber;
                _evlRequestDto.InsuranceTypeEnum = insuranceType;

                await navigationService.NavigateAsync("EvlRequestFiles", new NavigationParameters
                {
                    { "EvlRequestDto", _evlRequestDto }
                });

            }, () => SelectedCompany != null && SelectedVehicleKind != null);

            GoToNextPage.ObservesProperty(() => SelectedCompany);
            GoToNextPage.ObservesProperty(() => SelectedVehicleKind);
        }

        public async override Task OnNavigatedToAsync(NavigationParameters parameters)
        {
            if (parameters.GetNavigationMode() == NavigationMode.New)
            {
                using (_userDialogs.Loading(ConstantStrings.Loading))
                {
                    Companies = (await _odataClient.For<CompanyDto>("Companies").FindEntriesAsync()).ToArray();

                    SelectedCompany = Companies.FirstOrDefault();

                    VehicleKinds = (await _odataClient.For<VehicleKindDto>("VehicleKinds").FindEntriesAsync()).ToArray();

                    SelectedVehicleKind = VehicleKinds.FirstOrDefault();

                    InsuranceTypeEnums = EnumHelper<InsuranceType>.GetDisplayValues(insuranceType).ToArray();

                    SelectedInsuranceTypeEnum = InsuranceTypeEnums.FirstOrDefault();

                    _evlRequestDto = parameters.GetValue<EvlRequestDto>("EvlRequestDto");

                    Description = _evlRequestDto.Description;
                    OwnerFullName = _evlRequestDto.OwnerFullName;
                    OwnerMobileNumber = _evlRequestDto.OwnerMobileNumber;
                    AccidentDate = _dateTimeUtils.ConvertDateToShamsi(_evlRequestDto.AccidentDate);
                    SelectedCompany = Companies.FirstOrDefault(c => c.Id == _evlRequestDto.CompanyId);
                    SelectedVehicleKind = VehicleKinds.FirstOrDefault(v => v.Id == _evlRequestDto.VehicleKindId);
                    InsuranceNumber = _evlRequestDto.InsuranceNumber;

                    SelectedInsuranceTypeEnum = InsuranceTypeEnums.FirstOrDefault(i => i == EnumHelper<InsuranceType>.GetDisplayValue(_evlRequestDto.InsuranceTypeEnum));
                }
            }

            await base.OnNavigatedToAsync(parameters);
        }
    }
}

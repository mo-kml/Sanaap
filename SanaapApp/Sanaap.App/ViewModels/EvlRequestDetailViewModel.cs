using Acr.UserDialogs;
using Bit.ViewModel;
using Bit.ViewModel.Contracts;
using Prism.Navigation;
using Prism.Services;
using Sanaap.App.Dto;
using Sanaap.Constants;
using Sanaap.Service.Contracts;
using Simple.OData.Client;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Sanaap.App.ViewModels
{
    public class EvlRequestDetailViewModel : BitViewModelBase, IConfirmNavigation
    {
        private readonly IUserDialogs _userDialogs;
        private readonly IPageDialogService _pageDialogService;
        private readonly IODataClient _odataClient;
        private readonly IDateTimeUtils _dateTimeUtils;
        private readonly INavigationService _navigationService;
        private EvlRequestDto evlRequestDto;

        public CompanyDto[] Companies { get; set; }
        public CompanyDto SelectedCompany { get; set; }
        public VehicleKindDto[] VehicleKinds { get; set; }
        public VehicleKindDto SelectedVehicleKind { get; set; }
        public string AccidentDate { get; set; }
        public string InsuranceNumber { get; set; }
        public string OwnerFullName { get; set; }
        public string OwnerMobileNumber { get; set; }
        public string Description { get; set; }
        public BitDelegateCommand GoToNextPage { get; set; }

        public EvlRequestDetailViewModel(INavigationService navigationService,
            IPageDialogService pageDialogService,
            IClientAppProfile clientAppProfile,
            IUserDialogs userDialogs,
            IODataClient oDataClient,
            IDateTimeUtils dateTimeUtils,
            IDateTimeProvider dateTimeProvider)
        {
            _userDialogs = userDialogs;
            _pageDialogService = pageDialogService;
            _odataClient = oDataClient;
            _dateTimeUtils = dateTimeUtils;
            _navigationService = navigationService;

            AccidentDate = _dateTimeUtils.ConvertMiladiToShamsi(dateTimeProvider.GetCurrentUtcDateTime());

            GoToNextPage = new BitDelegateCommand(async () =>
            {
                if (!_dateTimeUtils.IsValidShamsiDate(AccidentDate))
                {
                    await pageDialogService.DisplayAlertAsync(ErrorMessages.Error, ErrorMessages.IncorrectDateFormat, ErrorMessages.Ok);
                    return;
                }

                SyncViewModelAndModel();

                await navigationService.NavigateAsync("EvlRequestFiles", new NavigationParameters
                {
                    { "EvlRequestDto", evlRequestDto }
                });

            }, () => SelectedCompany != null && SelectedVehicleKind != null);

            GoToNextPage.ObservesProperty(() => SelectedCompany);
            GoToNextPage.ObservesProperty(() => SelectedVehicleKind);
        }

        public override Task OnNavigatedFromAsync(NavigationParameters parameters)
        {
            SyncViewModelAndModel();
            parameters.Add("EvlRequestDto", evlRequestDto);
            return base.OnNavigatedFromAsync(parameters);
        }

        private void SyncViewModelAndModel()
        {
            evlRequestDto.CompanyId = SelectedCompany.Id;
            evlRequestDto.VehicleKindId = SelectedVehicleKind.Id;
            evlRequestDto.Description = Description;
            evlRequestDto.OwnerFullName = OwnerFullName;
            evlRequestDto.OwnerMobileNumber = OwnerMobileNumber;
            evlRequestDto.AccidentDate = _dateTimeUtils.ConvertShamsiToMiladi(AccidentDate);
            evlRequestDto.InsuranceNumber = InsuranceNumber;
        }

        public async override Task OnNavigatedToAsync(NavigationParameters parameters)
        {
            evlRequestDto = parameters.GetValue<EvlRequestDto>("EvlRequestDto");

            if (evlRequestDto.CompanyId == 0)
            {
                using (_userDialogs.Loading(ConstantStrings.Loading))
                    await LoadInitialData();
            }
            else
            {
                if (Companies == null)
                    Companies = (await _odataClient.For<CompanyDto>("Companies").FindEntriesAsync()).ToArray();
                SelectedCompany = Companies.FirstOrDefault(c => c.Id == evlRequestDto.CompanyId);
                if (VehicleKinds == null)
                    VehicleKinds = (await _odataClient.For<VehicleKindDto>("VehicleKinds").FindEntriesAsync()).ToArray();
                SelectedVehicleKind = VehicleKinds.FirstOrDefault(v => v.Id == evlRequestDto.VehicleKindId);
                AccidentDate = _dateTimeUtils.ConvertMiladiToShamsi(evlRequestDto.AccidentDate);
                InsuranceNumber = evlRequestDto.InsuranceNumber;
                OwnerFullName = evlRequestDto.OwnerFullName;
                OwnerMobileNumber = evlRequestDto.OwnerMobileNumber;
                Description = evlRequestDto.Description;
            }

            await base.OnNavigatedToAsync(parameters);
        }

        public async Task LoadInitialData()
        {
            Companies = (await _odataClient.For<CompanyDto>("Companies").FindEntriesAsync()).ToArray();

            VehicleKinds = (await _odataClient.For<VehicleKindDto>("VehicleKinds").FindEntriesAsync()).ToArray();

            AccidentDate = _dateTimeUtils.ConvertMiladiToShamsi(DateTimeOffset.UtcNow);
        }

        public bool CanNavigate(NavigationParameters parameters)
        {
            if (!_dateTimeUtils.IsValidShamsiDate(AccidentDate))
            {
                _pageDialogService.DisplayAlertAsync(ErrorMessages.Error, ErrorMessages.IncorrectDateFormat, ErrorMessages.Ok);
                return false;
            }
            else return true;
        }
    }
}

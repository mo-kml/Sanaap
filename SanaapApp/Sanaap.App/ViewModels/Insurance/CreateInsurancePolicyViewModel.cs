using Acr.UserDialogs;
using Bit.ViewModel;
using Prism.Navigation;
using Prism.Services;
using Sanaap.App.Helpers.Contracts;
using Sanaap.App.ItemSources;
using Sanaap.App.Services.Contracts;
using Sanaap.Constants;
using Sanaap.Dto;
using Sanaap.Enums;
using Sanaap.Service.Contracts;
using Simple.OData.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Sanaap.App.ViewModels.Insurance
{
    public class CreateInsurancePolicyViewModel : BitViewModelBase
    {
        private EditMethod method;
        private readonly IODataClient _oDataClient;
        private readonly HttpClient _httpClient;
        private readonly IInitialDataService _initialDataService;
        private readonly IUserDialogs _userDialogs;
        private readonly ILicenseHelper _licenseHelper;
        private IPageDialogService _dialogService;
        private IDateHelper _dateHelper;
        public CreateInsurancePolicyViewModel(
            IUserDialogs userDialogs,
            HttpClient httpClient,
            IInitialDataService initialDataService,
            IODataClient oDataClient,
            IInsuranceValidator insuranceValidator,
            IPageDialogService dialogService,
            IPolicyService policyService,
            ILicenseHelper licenseHelper,
            IDateHelper dateHelper,
            ISanaapAppTranslateService translateService
            )
        {
            _oDataClient = oDataClient;
            _userDialogs = userDialogs;
            _initialDataService = initialDataService;
            _licenseHelper = licenseHelper;
            _dialogService = dialogService;
            _dateHelper = dateHelper;

            foreach (InsuranceType item in (InsuranceType[])Enum.GetValues(typeof(InsuranceType)))
            {
                InsuranceTypes.Add(new InsuranceTypeItemSource
                {
                    InsuranceType = item,
                    InsuranceTypeName = EnumHelper<InsuranceType>.GetDisplayValue(item)
                });
            }

            SelectedInsuranceType = InsuranceTypes[0];

            Submit = new BitDelegateCommand(async () =>
              {

                  insuranceCancellationTokenSource?.Cancel();
                  insuranceCancellationTokenSource = new CancellationTokenSource();

                  using (_userDialogs.Loading(ConstantStrings.Loading, cancelText: ConstantStrings.Loading_Cancel, onCancel: insuranceCancellationTokenSource.Cancel))
                  {
                      if (SelectedCar == null)
                      {
                          await dialogService.DisplayAlertAsync(ConstantStrings.Error, ConstantStrings.CarIsNull, ConstantStrings.Ok);
                          return;
                      }
                      if (SelectedInsurer == null)
                      {
                          await dialogService.DisplayAlertAsync(ConstantStrings.Error, ConstantStrings.InsurerIsNull, ConstantStrings.Ok);
                          return;
                      }
                      if (SelectedAlphabet == null)
                      {
                          await dialogService.DisplayAlertAsync(ConstantStrings.Error, ConstantStrings.NumberPlateIsNotValid, ConstantStrings.Ok);
                          return;
                      }
                      if (SelectedColor == null)
                      {
                          await dialogService.DisplayAlertAsync(ConstantStrings.Error, ConstantStrings.ColorIsNull, ConstantStrings.Ok);
                          return;
                      }

                      if (SelectedDate == null)
                      {
                          await dialogService.DisplayAlertAsync(ConstantStrings.Error, ConstantStrings.ExpirationDateIsNotValid, ConstantStrings.Ok);
                          return;
                      }

                      License.Alphabet = SelectedAlphabet.Name;

                      Policy.ExpirationDate = new DateTimeOffset((DateTime)SelectedDate, DateTimeOffset.Now.Offset);

                      if (licenseHelper.ConvertToPlateNumber(License, out string licensePlate))
                      {
                          Policy.PlateNumber = licensePlate;
                      }
                      else
                      {
                          return;
                      }

                      if (!insuranceValidator.IsValid(Policy, out string errorMessage))
                      {
                          await dialogService.DisplayAlertAsync(string.Empty, translateService.Translate(errorMessage), ConstantStrings.Ok);
                          return;
                      }

                      Policy.ColorId = SelectedColor.PrmID;
                      Policy.CarId = SelectedCar.PrmID;
                      Policy.InsuranceType = SelectedInsuranceType.InsuranceType;
                      Policy.InsurerId = SelectedInsurer.ID;


                      policyCancellationTokenSource?.Cancel();
                      policyCancellationTokenSource = new CancellationTokenSource();

                      using (userDialogs.Loading(ConstantStrings.Loading, cancelText: ConstantStrings.Loading_Cancel, onCancel: policyCancellationTokenSource.Cancel))
                      {
                          if (method == EditMethod.Create)
                          {
                              await policyService.AddAsync(Policy);
                          }
                          else
                          {
                              await policyService.UpdateAsync(Policy);
                          }
                      }
                  }
                  await dialogService.DisplayAlertAsync(string.Empty, ConstantStrings.SuccessfulProcess, ConstantStrings.Ok);

                  await NavigationService.GoBackAsync();

              });


            SelectInsurer = new BitDelegateCommand<InsurersItemSource>(async (parameter) =>
            {
                foreach (InsurersItemSource insurer in Insurers)
                {
                    insurer.IsSelected = false;
                }

                parameter.IsSelected = true;

                SelectedInsurer = parameter;
            });
        }


        public override async Task OnNavigatedToAsync(INavigationParameters parameters)
        {
            insuranceCancellationTokenSource?.Cancel();
            insuranceCancellationTokenSource = new CancellationTokenSource();

            using (_userDialogs.Loading(ConstantStrings.Loading, cancelText: ConstantStrings.Loading_Cancel, onCancel: insuranceCancellationTokenSource.Cancel))
            {
                await syncData();

                parameters.TryGetValue("Method", out method);

                if (parameters.TryGetValue("Policy", out InsurancePolicyDto policy))
                {
                    Policy = new InsurancePolicyDto
                    {
                        CarId = policy.CarId,
                        ChasisNo = policy.ChasisNo,
                        ColorId = policy.ColorId,
                        CustomerId = policy.CustomerId,
                        Id = policy.Id,
                        InsuranceType = policy.InsuranceType,
                        InsurerId = policy.InsurerId,
                        InsurerNo = policy.InsurerNo,
                        ExpirationDate = policy.ExpirationDate,
                        PlateNumber = policy.PlateNumber,
                        VIN = policy.VIN
                    };

                    SelectedDate = Policy.ExpirationDate.DateTime;
                    SelectedColor = Colors.FirstOrDefault(c => c.PrmID == Policy.ColorId);
                    SelectedCar = Cars.FirstOrDefault(c => c.PrmID == Policy.CarId);
                    SelectedInsuranceType = InsuranceTypes.FirstOrDefault(c => c.InsuranceType == Policy.InsuranceType);
                    SelectedInsurer = Insurers.FirstOrDefault(c => c.ID == Policy.InsurerId);
                    License = _licenseHelper.ConvertToItemSource(policy.PlateNumber);
                    SelectedAlphabet = Alphabets.FirstOrDefault(a => a.Name == License.Alphabet);
                }
            }
        }

        public async Task syncData()
        {
            Cars = new ObservableCollection<ExternalEntityDto>(await _initialDataService.GetCars());

            Colors = new ObservableCollection<ExternalEntityDto>(await _initialDataService.GetColors());

            Insurers = new ObservableCollection<InsurersItemSource>(await _initialDataService.GetInsurers());

            Alphabets = new ObservableCollection<ExternalEntityDto>(await _initialDataService.GetAlphabets());
        }

        public InsurancePolicyDto Policy { get; set; } = new InsurancePolicyDto();

        public LicensePlateItemSource License { get; set; } = new LicensePlateItemSource();

        public BitDelegateCommand Submit { get; set; }

        public BitDelegateCommand SearchInCars { get; set; }

        public CancellationTokenSource insuranceCancellationTokenSource { get; set; }

        public ObservableCollection<ExternalEntityDto> Colors { get; set; }

        public ObservableCollection<ExternalEntityDto> Cars { get; set; }

        public ObservableCollection<InsurersItemSource> Insurers { get; set; }

        public InsurersItemSource SelectedInsurer { get; set; }

        public List<InsuranceTypeItemSource> InsuranceTypes { get; set; } = new List<InsuranceTypeItemSource>();

        public ObservableCollection<ExternalEntityDto> Alphabets { get; set; }

        public InsuranceTypeItemSource SelectedInsuranceType { get; set; }

        public ExternalEntityDto SelectedColor { get; set; }

        public ExternalEntityDto SelectedCar { get; set; }

        public ExternalEntityDto SelectedAlphabet { get; set; }

        public BitDelegateCommand<InsurersItemSource> SelectInsurer { get; set; }

        public CancellationTokenSource policyCancellationTokenSource { get; set; }

        public string Month { get; set; }

        public string Year { get; set; }

        public string Day { get; set; }

        public DateTime? SelectedDate { get; set; }

        public async void OnSelectedDateChanged()
        {
            if (SelectedDate == null)
            {
                return;
            }

            if (SelectedDate.Value.Date <= DateTime.Now)
            {
                await _dialogService.DisplayAlertAsync(ConstantStrings.Error, ConstantStrings.ExpirationDateIsNotValid, ConstantStrings.Ok);

                Year = string.Empty;
                Month = string.Empty;
                Day = string.Empty;

                SelectedDate = null;

                return;
            }

            _dateHelper.ToPersianLongDate(SelectedDate.Value, out string year, out string month, out string day);

            Year = year;
            Month = month;
            Day = day;
        }
    }
    public class InsuranceTypeItemSource
    {
        public InsuranceType InsuranceType { get; set; }

        public string InsuranceTypeName { get; set; }
    }
}

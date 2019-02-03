using Acr.UserDialogs;
using Bit.ViewModel;
using Prism.Navigation;
using Prism.Services;
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
        public CreateInsurancePolicyViewModel(
            IUserDialogs userDialogs,
            HttpClient httpClient,
            IInitialDataService initialDataService,
            IODataClient oDataClient,
            IInsuranceValidator insuranceValidator,
            IPageDialogService pageDialogService,
            IPolicyService policyService,

            ISanaapAppTranslateService translateService
            )
        {
            _oDataClient = oDataClient;
            _userDialogs = userDialogs;
            _initialDataService = initialDataService;



            foreach (InsuranceType item in (InsuranceType[])Enum.GetValues(typeof(InsuranceType)))
            {
                InsuranceTypes.Add(new InsuranceTypeItemSource
                {
                    InsuranceType = item,
                    InsuranceTypeName = EnumHelper<InsuranceType>.GetDisplayValue(item)
                });
            }

            Submit = new BitDelegateCommand(async () =>
              {

                  insuranceCancellationTokenSource?.Cancel();
                  insuranceCancellationTokenSource = new CancellationTokenSource();

                  using (_userDialogs.Loading(ConstantStrings.Loading, cancelText: ConstantStrings.Loading_Cancel, onCancel: insuranceCancellationTokenSource.Cancel))
                  {
                      if (!insuranceValidator.IsValid(Policy, out string errorMessage))
                      {
                          await pageDialogService.DisplayAlertAsync(string.Empty, translateService.Translate(errorMessage), ConstantStrings.Ok);
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
                  await pageDialogService.DisplayAlertAsync(string.Empty, ConstantStrings.SuccessfulProcess, ConstantStrings.Ok);

                  await NavigationService.GoBackAsync();

              }, () => SelectedCar != null && SelectedColor != null && SelectedInsuranceType != null && SelectedInsurer != null);
            Submit.ObservesProperty(() => SelectedCar);
            Submit.ObservesProperty(() => SelectedColor);
            Submit.ObservesProperty(() => SelectedInsuranceType);
            Submit.ObservesProperty(() => SelectedInsurer);

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
                        PlateNumber = policy.PlateNumber,
                        VIN = policy.VIN
                    };

                    SelectedColor = Colors.FirstOrDefault(c => c.PrmID == Policy.ColorId);
                    SelectedCar = Cars.FirstOrDefault(c => c.PrmID == Policy.CarId);
                    SelectedInsuranceType = InsuranceTypes.FirstOrDefault(c => c.InsuranceType == Policy.InsuranceType);
                    SelectedInsurer = Insurers.FirstOrDefault(c => c.ID == Policy.InsurerId);
                    SelectedInsurer.IsSelected = true;
                }

                if (method == EditMethod.Create)
                {
                    SubmitButtonText = "اضافه کردن";
                }
                else
                {
                    SubmitButtonText = "ویرایش";
                }
            }
        }

        public async Task syncData()
        {
            Cars = new ObservableCollection<ExternalEntityDto>(await _initialDataService.GetCars());

            Colors = new ObservableCollection<ExternalEntityDto>(await _initialDataService.GetColors());

            Insurers = new ObservableCollection<InsurersItemSource>(await _initialDataService.GetInsurers());
        }

        public InsurancePolicyDto Policy { get; set; } = new InsurancePolicyDto();

        public BitDelegateCommand Submit { get; set; }

        public BitDelegateCommand SearchInCars { get; set; }

        public CancellationTokenSource insuranceCancellationTokenSource { get; set; }

        public string CarSearchText { get; set; }

        public string SubmitButtonText { get; set; }

        public ObservableCollection<ExternalEntityDto> Colors { get; set; }

        public ObservableCollection<ExternalEntityDto> Cars { get; set; }

        public ObservableCollection<InsurersItemSource> Insurers { get; set; }

        public InsurersItemSource SelectedInsurer { get; set; }

        public List<InsuranceTypeItemSource> InsuranceTypes { get; set; } = new List<InsuranceTypeItemSource>();

        public InsuranceTypeItemSource SelectedInsuranceType { get; set; }

        public ExternalEntityDto SelectedColor { get; set; }

        public ExternalEntityDto SelectedCar { get; set; }


        public BitDelegateCommand<InsurersItemSource> SelectInsurer { get; set; }

        public CancellationTokenSource policyCancellationTokenSource { get; set; }
    }
    public class InsuranceTypeItemSource
    {
        public InsuranceType InsuranceType { get; set; }

        public string InsuranceTypeName { get; set; }
    }
}

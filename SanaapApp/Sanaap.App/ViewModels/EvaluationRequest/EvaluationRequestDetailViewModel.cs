using Acr.UserDialogs;
using Bit.ViewModel;
using Prism.Events;
using Prism.Navigation;
using Prism.Services;
using Sanaap.App.Helpers.Contracts;
using Sanaap.App.ItemSources;
using Sanaap.App.Services.Contracts;
using Sanaap.App.Views.EvaluationRequest;
using Sanaap.Constants;
using Sanaap.Dto;
using Sanaap.Enums;
using Sanaap.Service.Contracts;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sanaap.App.ViewModels.EvaluationRequest
{
    public class EvaluationRequestDetailViewModel : BitViewModelBase
    {
        private readonly IUserDialogs _userDialogs;
        private IInitialDataService _initialDataService;
        public EvaluationRequestDetailViewModel(
            IUserDialogs userDialogs,
            IEventAggregator eventAggregator,
            IInitialDataService initialDataService,
            IEvlRequestValidator evlRequestValidator,
            IPageDialogService dialogService,
            ILicenseHelper licenseHelper,
            ISanaapAppTranslateService translateService)
        {
            _userDialogs = userDialogs;
            _initialDataService = initialDataService;


            SelectFromInsurances = new BitDelegateCommand(async () =>
              {
                  await NavigationService.GoBackAsync(new NavigationParameters {
                      {"IsOpenInsurance",true },
                      {nameof(Request),Request },
                    });
              });

            Submit = new BitDelegateCommand(async () =>
              {
                  requestCancellationTokenSource?.Cancel();
                  requestCancellationTokenSource = new CancellationTokenSource();

                  using (_userDialogs.Loading(ConstantStrings.Loading, cancelText: ConstantStrings.Loading_Cancel, onCancel: requestCancellationTokenSource.Cancel))
                  {
                      Request.CarId = SelectedCar.PrmID;
                      Request.InsurerId = SelectedInsurer.ID;

                      License.Alphabet = SelectedAlphabet.Name;
                      if (licenseHelper.ConvertToPlateNumber(License, out string licensePlate))
                      {
                          Request.PlateNumber = licensePlate;
                      }
                      else
                      {
                          return;
                      }

                      if (!evlRequestValidator.IsDetailValid(Request, out string message))
                      {
                          await dialogService.DisplayAlertAsync(string.Empty, translateService.Translate(message), ConstantStrings.Ok);
                          return;
                      }

                      INavigationParameters Parameters = new NavigationParameters();
                      Parameters.Add(nameof(Request), Request);

                      if (Request.InsuranceType == InsuranceType.Badane)
                      {
                          await NavigationService.NavigateAsync(nameof(EvaluationRequestDescriptionView), Parameters);
                      }
                      else
                      {
                          await NavigationService.NavigateAsync(nameof(EvaluationRequestLostDetailView), Parameters);
                      }
                  }
              }, () => SelectedCar != null && SelectedInsurer != null && SelectedAlphabet != null);
            Submit.ObservesProperty(() => SelectedCar);
            Submit.ObservesProperty(() => SelectedInsurer);
            Submit.ObservesProperty(() => SelectedAlphabet);
        }
        public override async Task OnNavigatedToAsync(INavigationParameters parameters)
        {
            if (parameters.TryGetValue(nameof(InsuranceType), out InsuranceType insuranceType))
            {
                Request.InsuranceType = insuranceType;

                Request.IsSales = insuranceType == InsuranceType.Sales;
            }

            requestCancellationTokenSource?.Cancel();
            requestCancellationTokenSource = new CancellationTokenSource();

            if (parameters.TryGetValue(nameof(EvlRequestItemSource), out EvlRequestItemSource requestItemSource))
            {
                Request = requestItemSource;
            }

            using (_userDialogs.Loading(ConstantStrings.Loading, cancelText: ConstantStrings.Loading_Cancel, onCancel: requestCancellationTokenSource.Cancel))
            {
                await syncData();
            }

            if (parameters.TryGetValue("Insurance", out PolicyItemSource policy))
            {
                CustomerDto customer = await _initialDataService.GetCurrentUserInfo();

                insuranceType = Request.InsuranceType;

                Request = new EvlRequestItemSource
                {
                    OwnerFirstName = customer.FirstName,
                    OwnerLastName = customer.LastName,
                    InsurerNo = policy.InsurerNo,
                    PlateNumber = policy.PlateNumber,
                    InsuranceType = insuranceType,
                    IsSales = insuranceType == InsuranceType.Sales
                };

                SelectedInsurer = Insurers.FirstOrDefault(i => i.ID == policy.InsurerId);

                SelectedCar = Cars.FirstOrDefault(c => c.PrmID == policy.CarId);

                License = policy.LicensePlateItemSource;

                SelectedAlphabet = Alphabets.FirstOrDefault(a => a.Name == License.Alphabet);
            }
        }

        public async Task syncData()
        {
            Cars = new ObservableCollection<ExternalEntityDto>(await _initialDataService.GetCars());

            Alphabets = new ObservableCollection<ExternalEntityDto>(await _initialDataService.GetAlphabets());

            Insurers = new ObservableCollection<InsurersItemSource>(await _initialDataService.GetInsurers());
        }
        public EvlRequestItemSource Request { get; set; } = new EvlRequestItemSource();

        public ObservableCollection<ExternalEntityDto> Cars { get; set; }

        public ObservableCollection<ExternalEntityDto> Alphabets { get; set; }

        public ObservableCollection<InsurersItemSource> Insurers { get; set; }

        public InsurersItemSource SelectedInsurer { get; set; }

        public ExternalEntityDto SelectedAlphabet { get; set; }

        public ExternalEntityDto SelectedCar { get; set; }

        public BitDelegateCommand Submit { get; set; }

        public BitDelegateCommand SelectFromInsurances { get; set; }

        public CancellationTokenSource requestCancellationTokenSource { get; set; }

        public LicensePlateItemSource License { get; set; }

    }



}

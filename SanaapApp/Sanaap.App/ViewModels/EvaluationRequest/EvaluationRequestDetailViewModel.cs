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
        private readonly ILicenseHelper _licenseHelper;
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
            _licenseHelper = licenseHelper;

            SelectFromInsurances = new BitDelegateCommand(async () =>
              {
                  await NavigationService.GoBackAsync(new NavigationParameters {
                      {"IsOpenInsurance",true },
                      {nameof(Request),Request },
                    });
              });

            GoBack = new BitDelegateCommand(async () =>
            {
                await NavigationService.GoBackAsync();
            });

            GoToNextLevel = new BitDelegateCommand(async () =>
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
            GoToNextLevel.ObservesProperty(() => SelectedCar);
            GoToNextLevel.ObservesProperty(() => SelectedInsurer);
            GoToNextLevel.ObservesProperty(() => SelectedAlphabet);
        }
        public override async Task OnNavigatedToAsync(INavigationParameters parameters)
        {
            if (parameters.TryGetValue(nameof(InsuranceType), out InsuranceType insuranceType))
            {
                Request.InsuranceType = insuranceType;
            }

            if (parameters.GetNavigationMode() == NavigationMode.New)
            {



                requestCancellationTokenSource?.Cancel();
                requestCancellationTokenSource = new CancellationTokenSource();

                using (_userDialogs.Loading(ConstantStrings.Loading, cancelText: ConstantStrings.Loading_Cancel, onCancel: requestCancellationTokenSource.Cancel))
                {
                    await syncData();
                }

                if (parameters.TryGetValue(nameof(Request), out EvlRequestItemSource request))
                {
                    Request = request;

                    SelectedInsurer = Insurers.FirstOrDefault(i => i.ID == Request.InsurerId);

                    SelectedCar = Cars.FirstOrDefault(c => c.PrmID == Request.CarId);

                    License = _licenseHelper.ConvertToItemSource(Request.PlateNumber);

                    SelectedAlphabet = Alphabets.FirstOrDefault(a => a.Name == License.Alphabet);
                }
            }
            else if (parameters.GetNavigationMode() == NavigationMode.Back)
            {
                Request = parameters.GetValue<EvlRequestItemSource>(nameof(Request));

                SelectedInsurer = new InsurersItemSource();

                SelectedInsurer = Insurers.FirstOrDefault(i => i.ID == Request.InsurerId);

                //SelectedCar = Cars.FirstOrDefault(c => c.PrmID == Request.CarId);

                //License = _licenseHelper.ConvertToItemSource(Request.PlateNumber);

                //SelectedAlphabet = Alphabets.FirstOrDefault(a => a.Name == License.Alphabet);
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

        public BitDelegateCommand GoBack { get; set; }

        public InsurersItemSource SelectedInsurer { get; set; }

        public ExternalEntityDto SelectedAlphabet { get; set; }

        public ExternalEntityDto SelectedCar { get; set; }

        public BitDelegateCommand GoToNextLevel { get; set; }

        public BitDelegateCommand SelectFromInsurances { get; set; }

        public CancellationTokenSource requestCancellationTokenSource { get; set; }

        public LicensePlateItemSource License { get; set; }

    }



}

using Acr.UserDialogs;
using Bit.ViewModel;
using Prism.Navigation;
using Prism.Services;
using Sanaap.App.ItemSources;
using Sanaap.App.Services.Contracts;
using Sanaap.App.Views;
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
            IInitialDataService initialDataService,
            IEvlRequestValidator evlRequestValidator,
            IPageDialogService dialogService,
            ISanaapAppTranslateService translateService,
            INavigationService navigationService)
        {
            _userDialogs = userDialogs;
            _initialDataService = initialDataService;

            SelectInsurer = new BitDelegateCommand<InsurersItemSource>(async (parameter) =>
            {
                foreach (InsurersItemSource insurer in Insurers)
                {
                    insurer.IsSelected = false;
                }

                parameter.IsSelected = true;

                SelectedInsurer = parameter;
            });

            SelectFromInsurances = new BitDelegateCommand(async () =>
              {
                  NavigationParameters parameters = new NavigationParameters();
                  parameters.Add("Selective", true);

                  await navigationService.NavigateAsync("InsurancePolicyList", parameters);
              });

            SelectViewPlace = new BitDelegateCommand(async () =>
              {
                  requestCancellationTokenSource?.Cancel();
                  requestCancellationTokenSource = new CancellationTokenSource();

                  using (_userDialogs.Loading(ConstantStrings.Loading, cancelText: ConstantStrings.Loading_Cancel, onCancel: requestCancellationTokenSource.Cancel))
                  {
                      Request.CarId = SelectedCar.PrmID;
                      Request.InsurerId = SelectedInsurer.ID;
                      Request.LostCarId = LostSelectedCar == null ? 0 : LostSelectedCar.PrmID;

                      if (!evlRequestValidator.IsValid(Request, out string message))
                      {
                          await dialogService.DisplayAlertAsync(string.Empty, translateService.Translate(message), ConstantStrings.Ok);
                          return;
                      }

                      NavigationParameters Parameters = new NavigationParameters();
                      Parameters.Add(nameof(EvlRequestItemSource), Request);
                      Parameters.Add("NextPage", "EvlRequestFile");

                      await navigationService.NavigateAsync(nameof(MapView), Parameters);
                  }
              }, () => SelectedCar != null && SelectedInsurer != null);
            SelectViewPlace.ObservesProperty(() => SelectedCar);
            SelectViewPlace.ObservesProperty(() => SelectedInsurer);
        }
        public override async Task OnNavigatedToAsync(NavigationParameters parameters)
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

            if (parameters.TryGetValue("Policy", out PolicyItemSource policy))
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
                SelectedInsurer.IsSelected = true;

                SelectedCar = Cars.FirstOrDefault(c => c.PrmID == policy.CarId);
            }
        }

        public async Task syncData()
        {
            Cars = new ObservableCollection<ExternalEntityDto>(await _initialDataService.GetCars());

            Insurers = new ObservableCollection<InsurersItemSource>(await _initialDataService.GetInsurers());
        }
        public EvlRequestItemSource Request { get; set; } = new EvlRequestItemSource();

        public ObservableCollection<ExternalEntityDto> Cars { get; set; }

        public ObservableCollection<InsurersItemSource> Insurers { get; set; }

        public InsurersItemSource SelectedInsurer { get; set; }

        public ExternalEntityDto SelectedCar { get; set; }

        public ExternalEntityDto LostSelectedCar { get; set; }

        public BitDelegateCommand<InsurersItemSource> SelectInsurer { get; set; }

        public BitDelegateCommand SelectViewPlace { get; set; }

        public BitDelegateCommand SelectFromInsurances { get; set; }

        public CancellationTokenSource requestCancellationTokenSource { get; set; }
    }
}

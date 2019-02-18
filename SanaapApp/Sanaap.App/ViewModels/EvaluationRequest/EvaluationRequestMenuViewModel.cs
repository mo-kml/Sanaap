using Acr.UserDialogs;
using Bit.ViewModel;
using Prism.Events;
using Prism.Navigation;
using Prism.Services;
using Sanaap.App.Events;
using Sanaap.App.ItemSources;
using Sanaap.App.Services.Contracts;
using Sanaap.App.Views.EvaluationRequest;
using Sanaap.Constants;
using Sanaap.Dto;
using Sanaap.Enums;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace Sanaap.App.ViewModels.EvaluationRequest
{
    public class EvaluationRequestMenuViewModel : BitViewModelBase
    {
        private readonly IEventAggregator _eventAggregator;
        private IPolicyService _policyService;
        private EvlRequestItemSource _request;
        private IUserDialogs _userDialogs;
        public EvaluationRequestMenuViewModel(IEventAggregator eventAggregator, IPolicyService policyService, IUserDialogs userDialogs, IInitialDataService initialDataService, IPageDialogService dialogService)
        {
            _policyService = policyService;
            _userDialogs = userDialogs;

            _eventAggregator = eventAggregator;

            EvlRequestBadane = new BitDelegateCommand(async () =>
              {
                  await NavigationService.NavigateAsync(nameof(EvaluationRequestDetailView), new NavigationParameters
                  {
                    { nameof(InsuranceType), InsuranceType.Badane }
                  });
              });

            EvlRequestSales = new BitDelegateCommand(async () =>
            {
                await NavigationService.NavigateAsync(nameof(EvaluationRequestDetailView), new NavigationParameters
                {
                    { nameof(InsuranceType), InsuranceType.Sales }
                });
            });

            #region insurancePopup

            SelectPolicy = new BitDelegateCommand<PolicyItemSource>(async (policy) =>
            {
                using (_userDialogs.Loading(ConstantStrings.Loading))
                {
                    CustomerDto customer = await initialDataService.GetCurrentUserInfo();

                    eventAggregator.GetEvent<OpenInsurancePopupEvent>().Publish(new OpenInsurancePopupEvent());

                    _request.InsurerNo = policy.InsurerNo;
                    _request.InsurerId = policy.InsurerId;
                    _request.CarId = policy.CarId;
                    _request.PlateNumber = policy.PlateNumber;
                    _request.OwnerFirstName = customer.FirstName;
                    _request.OwnerLastName = customer.LastName;

                    await NavigationService.NavigateAsync(nameof(EvaluationRequestDetailView), new NavigationParameters {
                        {"Request",_request },
                });
                }
            });

            #endregion
        }

        public override async Task OnNavigatedToAsync(INavigationParameters parameters)
        {
            if (parameters.TryGetValue("IsOpenInsurance", out bool isOpen))
            {
                _eventAggregator.GetEvent<OpenInsurancePopupEvent>().Publish(new OpenInsurancePopupEvent());

                _request = parameters.GetValue<EvlRequestItemSource>("Request");

                insuranceCancellationTokenSource?.Cancel();
                insuranceCancellationTokenSource = new CancellationTokenSource();

                using (_userDialogs.Loading(ConstantStrings.Loading, cancelText: ConstantStrings.Loading_Cancel, onCancel: insuranceCancellationTokenSource.Cancel))
                {
                    await loadInsurances(_request.InsuranceType);
                }
            }
        }
        public BitDelegateCommand EvlRequestSales { get; set; }

        public BitDelegateCommand EvlRequestBadane { get; set; }


        #region insurancePopup

        public ObservableCollection<PolicyItemSource> Insurances { get; set; }

        public BitDelegateCommand<PolicyItemSource> SelectPolicy { get; set; }

        public CancellationTokenSource insuranceCancellationTokenSource { get; set; }

        public async Task loadInsurances(InsuranceType insuranceType)
        {
            Insurances = new ObservableCollection<PolicyItemSource>(await _policyService.LoadAllInsurancesByType(insuranceType));
        }
        #endregion
    }
}

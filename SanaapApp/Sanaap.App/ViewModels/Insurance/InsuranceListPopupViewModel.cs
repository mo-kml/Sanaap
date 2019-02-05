using Acr.UserDialogs;
using Bit.ViewModel;
using Prism.Events;
using Sanaap.App.Events;
using Sanaap.App.ItemSources;
using Sanaap.App.Services.Contracts;
using Sanaap.Constants;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace Sanaap.App.ViewModels.Insurance
{
    public class InsuranceListPopupViewModel : BitViewModelBase
    {
        private readonly IPolicyService _policyService;
        private readonly IUserDialogs _userDialogs;
        private readonly SubscriptionToken SubscriptionToken;
        public InsuranceListPopupViewModel(IEventAggregator eventAggregator, IPolicyService policyService, IUserDialogs userDialogs)
        {
            _policyService = policyService;
            _userDialogs = userDialogs;

            EvlRequestItemSource _request = new EvlRequestItemSource();

            SubscriptionToken = eventAggregator.GetEvent<InsuranceEvent>().SubscribeAsync(async (request) =>
              {
                  _request = request;

                  insuranceCancellationTokenSource?.Cancel();
                  insuranceCancellationTokenSource = new CancellationTokenSource();

                  using (_userDialogs.Loading(ConstantStrings.Loading, cancelText: ConstantStrings.Loading_Cancel, onCancel: insuranceCancellationTokenSource.Cancel))
                  {
                      await loadInsurances();
                  }
              }, keepSubscriberReferenceAlive: true, threadOption: ThreadOption.UIThread);

            SelectPolicy = new BitDelegateCommand<PolicyItemSource>(async (policy) =>
            {
                eventAggregator.GetEvent<OpenInsurancePopupEvent>().Publish(new OpenInsurancePopupEvent());

                //await NavigationService.NavigateAsync(nameof(EvaluationRequestView), new NavigationParameters {
                //    { "Insurance",policy},
                //    {nameof(EvlRequestItemSource),_request }
                //});
            });
        }

        public ObservableCollection<PolicyItemSource> Insurances { get; set; }

        public BitDelegateCommand<PolicyItemSource> SelectPolicy { get; set; }

        public CancellationTokenSource insuranceCancellationTokenSource { get; set; }

        public async Task loadInsurances()
        {
            Insurances = new ObservableCollection<PolicyItemSource>(await _policyService.LoadAllInsurances());
        }
    }
}

using Acr.UserDialogs;
using Bit.ViewModel;
using Bit.ViewModel.Contracts;
using Prism.Events;
using Prism.Ioc;
using Prism.Navigation;
using PropertyChanged;
using Sanaap.App.Events;
using Sanaap.App.ItemSources;
using Sanaap.App.Services.Contracts;
using Sanaap.App.Views.EvaluationRequest;
using Sanaap.Constants;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Sanaap.App.ViewModels.Insurance
{
    public class InsuranceListPopupViewModelLocator
    {
        public InsuranceListPopupViewModel InsuranceListPopupViewModel => ((App)Application.Current).Container.Resolve<InsuranceListPopupViewModel>();
    }

    [AddINotifyPropertyChangedInterface]
    public class InsuranceListPopupViewModel
    {
        private readonly IPolicyService _policyService;
        private readonly IUserDialogs _userDialogs;
        private readonly SubscriptionToken SubscriptionToken;
        public InsuranceListPopupViewModel(IEventAggregator eventAggregator, IPolicyService policyService, IUserDialogs userDialogs, INavService navService)
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

                await navService.NavigateAsync(nameof(EvaluationRequestView), new NavigationParameters {
                    { "Insurance",policy},
                    {nameof(EvlRequestItemSource),_request }
                });
            });

            Test = new BitDelegateCommand(async () =>
              {

              });
        }
        public ObservableCollection<PolicyItemSource> Insurances { get; set; }

        public BitDelegateCommand<PolicyItemSource> SelectPolicy { get; set; }

        public BitDelegateCommand Test { get; set; }

        public CancellationTokenSource insuranceCancellationTokenSource { get; set; }

        public async Task loadInsurances()
        {
            System.Collections.Generic.List<PolicyItemSource> insurances = await _policyService.LoadAllInsurances();
            Insurances = new ObservableCollection<PolicyItemSource>(insurances);
        }
    }
}

using Acr.UserDialogs;
using Bit.ViewModel;
using Prism.Events;
using Prism.Navigation;
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
        public InsuranceListPopupViewModel(IEventAggregator eventAggregator, IPolicyService policyService, IUserDialogs userDialogs)
        {
            _policyService = policyService;
            _userDialogs = userDialogs;

            SelectPolicy = new BitDelegateCommand<PolicyItemSource>(async (policy) =>
            {
                eventAggregator.GetEvent<InsuranceEvent>().Publish(new InsuranceEventArgs { Policy = policy });
            });
        }
        public override async Task OnNavigatedToAsync(NavigationParameters parameters)
        {
            insuranceCancellationTokenSource?.Cancel();
            insuranceCancellationTokenSource = new CancellationTokenSource();

            using (_userDialogs.Loading(ConstantStrings.Loading, cancelText: ConstantStrings.Loading_Cancel, onCancel: insuranceCancellationTokenSource.Cancel))
            {
                await loadInsurances();
            }
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

using Acr.UserDialogs;
using Bit.ViewModel;
using Prism.Navigation;
using Sanaap.App.ItemSources;
using Sanaap.App.Services.Contracts;
using Sanaap.App.Views.Insurance;
using Sanaap.Constants;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace Sanaap.App.ViewModels.Insurance
{
    public enum EditMethod
    {
        Create,
        Update
    }
    public class InsurancePolicyListViewModel : BitViewModelBase
    {
        private readonly IPolicyService _policyService;
        private readonly IUserDialogs _userDialogs;
        public InsurancePolicyListViewModel(IPolicyService policyService, IUserDialogs userDialogs)
        {
            _policyService = policyService;
            _userDialogs = userDialogs;

            CreatePolicy = new BitDelegateCommand(async () =>
              {
                  INavigationParameters parameters = new NavigationParameters();
                  parameters.Add("Method", EditMethod.Create);

                  await NavigationService.NavigateAsync(nameof(CreateInsurancePolicyView));
              });

            ShowPolicy = new BitDelegateCommand<PolicyItemSource>(async (policy) =>
              {
                  INavigationParameters parameters = new NavigationParameters();

                  parameters.Add("Policy", policy);
                  parameters.Add("Method", EditMethod.Update);

                  await NavigationService.NavigateAsync(nameof(CreateInsurancePolicyView), parameters);
              });
        }
        public override async Task OnNavigatedToAsync(INavigationParameters parameters)
        {
            insuranceCancellationTokenSource?.Cancel();
            insuranceCancellationTokenSource = new CancellationTokenSource();

            using (_userDialogs.Loading(ConstantStrings.Loading, cancelText: ConstantStrings.Loading_Cancel, onCancel: insuranceCancellationTokenSource.Cancel))
            {
                await loadInsurances();
            }
        }
        public ObservableCollection<PolicyItemSource> Policies { get; set; }

        public BitDelegateCommand CreatePolicy { get; set; }

        public BitDelegateCommand<PolicyItemSource> ShowPolicy { get; set; }

        public CancellationTokenSource insuranceCancellationTokenSource { get; set; }

        public async Task loadInsurances()
        {
            Policies = new ObservableCollection<PolicyItemSource>(await _policyService.LoadAllInsurances());
        }

    }
}

using Acr.UserDialogs;
using Bit.ViewModel;
using Prism.Navigation;
using Sanaap.App.ItemSources;
using Sanaap.App.Services.Contracts;
using Sanaap.Constants;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace Sanaap.App.ViewModels.Insurance
{
    public class InsurancePolicyListViewModel : BitViewModelBase
    {
        private readonly IPolicyService _policyService;
        private readonly IUserDialogs _userDialogs;
        public InsurancePolicyListViewModel(INavigationService navigationService, IPolicyService policyService, IUserDialogs userDialogs)
        {
            _policyService = policyService;
            _userDialogs = userDialogs;

            CreatePolicy = new BitDelegateCommand(async () =>
              {
                  await navigationService.NavigateAsync("CreatePolicy");
              });

            ShowPolicy = new BitDelegateCommand<PolicyItemSource>(async (policy) =>
              {
                  NavigationParameters parameters = new NavigationParameters();
                  parameters.Add("Policy", policy);

                  await navigationService.NavigateAsync("CreatePolicy", parameters);
              });
        }
        public override async Task OnNavigatedToAsync(NavigationParameters parameters)
        {
            await loadInsurances();
        }
        public ObservableCollection<PolicyItemSource> Policies { get; set; }

        public BitDelegateCommand CreatePolicy { get; set; }

        public BitDelegateCommand<PolicyItemSource> ShowPolicy { get; set; }

        public CancellationTokenSource insuranceCancellationTokenSource { get; set; }

        public async Task loadInsurances()
        {

            try
            {
                insuranceCancellationTokenSource?.Cancel();
                insuranceCancellationTokenSource = new CancellationTokenSource();

                using (_userDialogs.Loading(ConstantStrings.Loading, cancelText: ConstantStrings.Loading_Cancel, onCancel: insuranceCancellationTokenSource.Cancel))
                {
                    Policies = new ObservableCollection<PolicyItemSource>(await _policyService.LoadAllInsurances());
                }
            }
            catch (Exception ex)
            {

            }

        }
    }


}

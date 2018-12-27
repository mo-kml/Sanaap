using Bit.ViewModel;
using Prism.Navigation;
using PropertyChanged;
using Sanaap.Dto;
using Sanaap.Enums;
using System;
using System.Collections.ObjectModel;

namespace Sanaap.App.ViewModels.Insurance
{
    public class InsurancePolicyListViewModel : BitViewModelBase
    {
        public InsurancePolicyListViewModel(INavigationService navigationService)
        {
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
        public ObservableCollection<PolicyItemSource> Policies { get; set; }

        public BitDelegateCommand CreatePolicy { get; set; }

        public BitDelegateCommand<PolicyItemSource> ShowPolicy { get; set; }
    }

    [AddINotifyPropertyChangedInterface]
    public class PolicyItemSource : InsurancePolicyDto
    {
        public string ColorName { get; set; }

        public string CarName { get; set; }

        public string InsuranceTypeName => EnumHelper<InsuranceType>.GetDisplayValue(InsuranceType);
    }
}

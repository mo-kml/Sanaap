using Bit.ViewModel;
using Bit.ViewModel.Contracts;
using Prism.Navigation;

namespace Sanaap.App.ViewModels
{
    public class ConfirmOtpViewModel : BitViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly ISecurityService _securityService;

        public virtual string ReceivedCode { get; set; }

        public virtual BitDelegateCommand Confirm { get; set; }

        public ConfirmOtpViewModel(INavigationService navigationService,
            ISecurityService securityService)
        {
            _navigationService = navigationService;
            _securityService = securityService;
        }

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            Confirm = new BitDelegateCommand(async () =>
            {
                await _securityService.LoginWithCredentials(parameters.GetValue<string>("nationalCode"), ReceivedCode, "SanaapResOwner", "secret");
                await _navigationService.NavigateAsync("Main");
            }, () => !string.IsNullOrEmpty(ReceivedCode) && ReceivedCode.Length == 4);

            Confirm.ObservesProperty(() => ReceivedCode);

            base.OnNavigatedTo(parameters);
        }
    }
}

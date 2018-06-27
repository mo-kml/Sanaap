using Bit.ViewModel;
using Bit.ViewModel.Contracts;
using Prism.Navigation;

namespace SanaapOperatorApp.MainModels
{
    public class MenuViewModel : BitViewModelBase
    {
        public BitDelegateCommand<string> GoToPage { get; set; }

        public BitDelegateCommand Logout { get; set; }

        public MenuViewModel(INavigationService navigationService,
            ISecurityService securityService)
        {
            GoToPage = new BitDelegateCommand<string>(async (page) =>
            {
                await navigationService.NavigateAsync(page);
            });

            Logout = new BitDelegateCommand(async () =>
            {
                await securityService.Logout();
                await navigationService.NavigateAsync("/Login");
            });
        }
    }
}

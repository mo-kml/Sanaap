using Bit.ViewModel;
using Prism.Navigation;

namespace Sanaap.App.ViewModels
{
    public class MenuViewModel : BitViewModelBase
    {
        public BitDelegateCommand GoToMySosRequests { get; set; }

        public MenuViewModel(INavigationService navigationService)
        {
            GoToMySosRequests = new BitDelegateCommand(async () =>
            {
                await navigationService.NavigateAsync("MySosRequests");
            });
        }
    }
}
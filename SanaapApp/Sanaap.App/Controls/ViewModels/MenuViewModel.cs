using Bit.ViewModel;
using Bit.ViewModel.Contracts;
using Prism.Events;
using Sanaap.App.Events;
using Sanaap.App.Views;
using Xamarin.Forms;

namespace Sanaap.App.Controls.ViewModels
{
    public class MenuViewModel : BitViewModelBase
    {
        public BitDelegateCommand<string> GoToPage { get; set; }

        public BitDelegateCommand Logout { get; set; }

        public BitDelegateCommand GoBack { get; set; }

        public AbsoluteLayout Menu { get; set; }

        public MenuViewModel(ISecurityService securityService, IEventAggregator eventAggregator)
        {
            GoToPage = new BitDelegateCommand<string>(async (page) =>
            {
                eventAggregator.GetEvent<ToggleMenuEvent>().Publish(Menu);

                await NavigationService.NavigateAsync(page);
            });

            Logout = new BitDelegateCommand(async () =>
            {
                eventAggregator.GetEvent<ToggleMenuEvent>().Publish(Menu);

                await securityService.Logout();
                await NavigationService.NavigateAsync($"/{nameof(NavigationPage)}/{nameof(LoginView)}");
            });

            GoBack = new BitDelegateCommand(async () =>
              {
                  eventAggregator.GetEvent<ToggleMenuEvent>().Publish(Menu);

                  await NavigationService.GoBackAsync();
              });

        }
        public BitDelegateCommand Submit { get; set; }

        public void Dispose()
        {

        }
    }
}

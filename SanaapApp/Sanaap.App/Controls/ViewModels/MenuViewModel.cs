using Bit.ViewModel;
using Prism.Navigation;
using System.Threading.Tasks;

namespace Sanaap.App.Controls.ViewModels
{
    public class MenuViewModel : BitViewModelBase
    {
        public override Task OnNavigatedToAsync(NavigationParameters parameters)
        {
            return base.OnNavigatedToAsync(parameters);
        }
        public MenuViewModel()
        {
            Submit = new BitDelegateCommand(async () =>
              {

              });
        }
        public BitDelegateCommand Submit { get; set; }
    }
}

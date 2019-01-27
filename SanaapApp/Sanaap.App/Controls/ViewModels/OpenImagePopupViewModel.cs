using Bit.ViewModel;
using Prism.Navigation;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Sanaap.App.Controls.ViewModels
{
    public class OpenImagePopupViewModel : BitViewModelBase
    {
        public ImageSource Image { get; set; }

        public BitDelegateCommand GoBack { get; set; }

        public OpenImagePopupViewModel()
        {
            GoBack = new BitDelegateCommand(async () =>
              {
                  await NavigationService.GoBackAsync();
              });
        }

        public override Task OnNavigatedToAsync(INavigationParameters parameters)
        {
            Image = parameters.GetValue<ImageSource>(nameof(Image));

            return base.OnNavigatedToAsync(parameters);
        }
    }
}

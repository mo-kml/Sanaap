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
            /*  Submit = new BitDelegateCommand(async () =>
                {

                });*/

            GoToMainPage = new BitDelegateCommand(async () =>
            {

            });

            GoToFiles = new BitDelegateCommand(async () =>
            {

            });

            GoToIns = new BitDelegateCommand(async () =>
            {

            });

            GoToNews = new BitDelegateCommand(async () =>
            {

            });

            GoToMessages = new BitDelegateCommand(async () =>
            {

            });

            GoToContactUs = new BitDelegateCommand(async () =>
            {

            });

            Exit = new BitDelegateCommand(async () =>
            {

            });


        }
        //public BitDelegateCommand Submit { get; set; }

        public BitDelegateCommand GoToMainPage { get; set; }

        public BitDelegateCommand GoToFiles { get; set; }

        public BitDelegateCommand GoToIns { get; set; }

        public BitDelegateCommand GoToNews { get; set; }

        public BitDelegateCommand GoToMessages { get; set; }

        public BitDelegateCommand GoToContactUs { get; set; }

        public BitDelegateCommand Exit { get; set; }

    }
}

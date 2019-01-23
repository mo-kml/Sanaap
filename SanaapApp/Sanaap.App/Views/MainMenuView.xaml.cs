using Acr.UserDialogs;
using Sanaap.App.Helpers;
using Xamarin.Forms;

namespace Sanaap.App.Views
{
    public partial class MainMenuView : ContentPage
    {
        private int count = 0;
        private readonly IAppUtilities _utility;
        private IUserDialogs _userDialogs;
        public MainMenuView(IAppUtilities utility, IUserDialogs userDialogs)
        {
            _utility = utility;
            _userDialogs = userDialogs;
            InitializeComponent();
        }

        protected override bool OnBackButtonPressed()
        {
            count++;
            if (count == 1)
            {
                _userDialogs.Toast("برای بستن برنامه یک بار دیگر بازگشت را بزنید");
            }
            if (count == 2)
            {
                _utility.Exit();
                return false;
            }
            else
            {
                return true;
            }
        }

        protected override void OnAppearing()
        {
            count = 0;
        }
    }
}

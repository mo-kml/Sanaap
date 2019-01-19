using Acr.UserDialogs;
using Sanaap.App.Helpers;
using Xamarin.Forms;

namespace Sanaap.App.Views
{
    public partial class MainView : ContentPage
    {
        private int count = 0;
        private readonly IAppUtilities _utility;
        public MainView(IAppUtilities utility)
        {

            InitializeComponent();
        }

        protected override bool OnBackButtonPressed()
        {
            count++;
            if (count == 1)
            {
                UserDialogs.Instance.Toast("برای بستن برنامه یک بار دیگر بازگشت را بزنید");
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

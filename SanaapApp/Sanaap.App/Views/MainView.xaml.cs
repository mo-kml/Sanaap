using Plugin.Toast;
using Sanaap.App.Helpers;
using Xamarin.Forms;

namespace Sanaap.App.Views
{
    public partial class MainView : ContentPage
    {
        int count = 0;
        private readonly IAppUtilities _utility;
        public MainView(IAppUtilities utility)
        {
            _utility = utility;
            InitializeComponent();
        }

        protected override bool OnBackButtonPressed()
        {
            count++;
            if (count == 1)
            {
                if (CrossToastPopUp.IsSupported)
                    CrossToastPopUp.Current.ShowToastMessage("برای بستن برنامه یک بار دیگر بازگشت را بزنید");
            }
            if (count == 2)
            {
                _utility.Exit();
                return false;
            }
            else return true;
        }

        protected override void OnAppearing()
        {
            count = 0;
        }
    }
}

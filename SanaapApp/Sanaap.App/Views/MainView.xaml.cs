using Plugin.Toast;
using Xamarin.Forms;

namespace Sanaap.App.Views
{
    public partial class MainView : ContentPage
    {
        int count = 0;

        public MainView()
        {
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
                return false;
            else return true;
        }

        protected override void OnAppearing()
        {
            count = 0;
        }
    }
}
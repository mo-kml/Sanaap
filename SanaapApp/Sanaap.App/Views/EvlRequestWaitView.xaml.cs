using Xamarin.Forms;

namespace Sanaap.App.Views
{
    public partial class EvlRequestWaitView : ContentPage
    {
        public EvlRequestWaitView()
        {
            InitializeComponent();

            NavigationPage.SetHasBackButton(this, false);
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}

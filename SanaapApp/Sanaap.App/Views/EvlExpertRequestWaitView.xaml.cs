
using Xamarin.Forms;

namespace Sanaap.App.Views
{
    public partial class EvlExpertRequestWaitView : ContentPage
    {
        public EvlExpertRequestWaitView()
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

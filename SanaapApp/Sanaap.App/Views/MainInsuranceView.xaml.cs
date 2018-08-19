using Sanaap.App.Helpers;
using Xamarin.Forms;

namespace Sanaap.App.Views
{
    public partial class MainInsuranceView : ContentPage
    {
        int count = 0;
        private readonly IAppUtilities _utility;
        public MainInsuranceView(IAppUtilities utility)
        {
            _utility = utility;
            //InitializeComponent();
        }

        protected override void OnAppearing()
        {
            count = 0;
        }
    }
}

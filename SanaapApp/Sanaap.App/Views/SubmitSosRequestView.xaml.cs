using Xamarin.Forms;

namespace Sanaap.App.Views
{
    public partial class SubmitSosRequestView : ContentPage
    {
        public SubmitSosRequestView()
        {
            InitializeComponent();
            map.UiSettings.MyLocationButtonEnabled = true;
        }
    }
}
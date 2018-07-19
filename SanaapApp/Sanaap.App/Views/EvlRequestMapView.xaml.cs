
using Xamarin.Forms;

namespace Sanaap.App.Views
{
    public partial class EvlRequestMapView : ContentPage
    {
        public EvlRequestMapView()
        {
            InitializeComponent();
            map.UiSettings.MyLocationButtonEnabled = true;
        }
    }
}

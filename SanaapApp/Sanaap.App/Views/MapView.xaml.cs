
using Xamarin.Forms;

namespace Sanaap.App.Views
{
    public partial class MapView : ContentPage
    {
        public MapView()
        {
            InitializeComponent();
            map.UiSettings.MyLocationButtonEnabled = true;
        }
    }
}

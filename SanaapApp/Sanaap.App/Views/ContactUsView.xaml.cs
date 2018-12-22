
using Xamarin.Forms;

namespace Sanaap.App.Views
{
    public partial class ContactUsView : ContentPage
    {
        public ContactUsView()
        {
            InitializeComponent();

            map.Pins.Add(new Xamarin.Forms.GoogleMaps.Pin
            {
                Label = "شرکت ایرانیان پوشش",
                Position = new Xamarin.Forms.GoogleMaps.Position(35.7232935, 51.4135174)
            });
        }
    }
}

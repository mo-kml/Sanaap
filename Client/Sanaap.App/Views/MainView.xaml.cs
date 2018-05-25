using Xamarin.Forms;

namespace Sanaap.App.Views
{
    public partial class MainView : ContentPage
	{
		public MainView ()
		{
			InitializeComponent ();
            imgIran.Source = ImageSource.FromResource("Sanaap.App.img.iran.png");
            imgPhone.Source = ImageSource.FromResource("Sanaap.App.img.phone.jpg");
        }
	}
}
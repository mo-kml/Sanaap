
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Sanaap.App.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuView : ContentView
    {
        public MenuView()
        {
            InitializeComponent();

            BindingContext = new Sanaap.App.Controls.ViewModels.MenuViewModel();
        }
    }
}

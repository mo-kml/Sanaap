using Syncfusion.SfNavigationDrawer.XForms;
using Xamarin.Forms;

namespace Sanaap.App.Controls
{
    public partial class MenuView : ContentView
    {
        public MenuView()
        {
            InitializeComponent();
        }

        public static BindableProperty TypeProperty = BindableProperty.Create(nameof(NavigationDrawer), typeof(SfNavigationDrawer), typeof(ContentPage), null, BindingMode.TwoWay);
        public SfNavigationDrawer NavigationDrawer
        {
            get => (SfNavigationDrawer)GetValue(TypeProperty);
            set => SetValue(TypeProperty, value);
        }
    }
}

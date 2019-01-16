using Sanaap.App.Controls.ViewModels;
using Syncfusion.SfNavigationDrawer.XForms;
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
        }

        public SfNavigationDrawer NavigationDrawer
        {
            get => (SfNavigationDrawer)GetValue(TypeProperty);
            set
            {
                SetValue(TypeProperty, value);

                ((MenuViewModel)BindingContext).NavigationDrawer = value;
            }
        }

        public static readonly BindableProperty TypeProperty =
            BindableProperty.Create("NavigationDrawer", typeof(SfNavigationDrawer), typeof(SfNavigationDrawer), null);
    }
}

using Sanaap.App.Controls.ViewModels;
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

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (BindingContext != null)
            {
                ((MenuViewModel)BindingContext).NavigationDrawer = NavigationDrawer;
            }
        }

        public static BindableProperty NavigationDrawerProperty = BindableProperty.Create(nameof(NavigationDrawer), typeof(SfNavigationDrawer), typeof(ContentPage), null, BindingMode.TwoWay);
        public SfNavigationDrawer NavigationDrawer
        {
            get => (SfNavigationDrawer)GetValue(NavigationDrawerProperty);
            set => SetValue(NavigationDrawerProperty, value);
        }
    }
}

using Sanaap.App.Controls.ViewModels;
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
                ((MenuViewModel)BindingContext).Menu = Menu;
            }
        }

        public static BindableProperty MenuProperty = BindableProperty.Create(nameof(Menu), typeof(AbsoluteLayout), typeof(ContentPage), null, BindingMode.TwoWay);
        public AbsoluteLayout Menu
        {
            get => (AbsoluteLayout)GetValue(MenuProperty);
            set => SetValue(MenuProperty, value);
        }
    }
}

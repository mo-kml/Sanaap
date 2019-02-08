
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Sanaap.App.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterTemplateView : ContentView
    {
        public MasterTemplateView()
        {
            InitializeComponent();
        }

        public static BindableProperty ContentPresenterProperty = BindableProperty.Create(nameof(ContentPresenter), typeof(ContentPresenter), typeof(MasterTemplateView), null, BindingMode.TwoWay);
        public ContentPresenter ContentPresenter
        {
            get => (ContentPresenter)GetValue(ContentPresenterProperty);
            set => SetValue(ContentPresenterProperty, value);
        }
    }
}

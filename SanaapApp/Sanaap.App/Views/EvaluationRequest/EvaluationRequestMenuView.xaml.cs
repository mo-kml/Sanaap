using Prism.Events;
using Sanaap.App.Events;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Sanaap.App.Views.EvaluationRequest
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EvaluationRequestMenuView : ContentPage
    {
        public EvaluationRequestMenuView(IEventAggregator eventAggregator)
        {
            InitializeComponent();

            eventAggregator.GetEvent<OpenInsurancePopupEvent>().SubscribeAsync(async (nothing) =>
            {
                navigationDrawer.ToggleDrawer();
            });
        }
    }
}

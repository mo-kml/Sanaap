
using Prism.Events;
using Rg.Plugins.Popup.Pages;
using Sanaap.App.Events;
using Syncfusion.SfNavigationDrawer.XForms;
using Xamarin.Forms.Xaml;

namespace Sanaap.App.Views.EvaluationRequest
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EvaluationRequestDetailView : PopupPage
    {
        public EvaluationRequestDetailView(IEventAggregator eventAggregator)
        {
            InitializeComponent();

            eventAggregator.GetEvent<InsuranceEvent>().SubscribeAsync(async (policy) =>
            {
                ((SfNavigationDrawer)FindByName("navigationDrawer")).ToggleDrawer();
            });
        }


    }
}

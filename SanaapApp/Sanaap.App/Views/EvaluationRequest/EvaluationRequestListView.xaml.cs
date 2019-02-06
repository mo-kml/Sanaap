using Prism.Events;
using Sanaap.App.Events;

using Xamarin.Forms;

namespace Sanaap.App.Views.EvaluationRequest
{

    public partial class EvaluationRequestListView : ContentPage
    {
        public EvaluationRequestListView(IEventAggregator eventAggregator)
        {
            InitializeComponent();

            eventAggregator.GetEvent<OpenInquiryPopup>().SubscribeAsync(async (nothing) =>
            {
                navigationDrawer.ToggleDrawer();
            }, keepSubscriberReferenceAlive: true, threadOption: ThreadOption.UIThread);
        }
    }
}

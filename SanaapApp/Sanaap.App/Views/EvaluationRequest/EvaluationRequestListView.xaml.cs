using Prism.Events;
using Sanaap.App.Events;

namespace Sanaap.App.Views.EvaluationRequest
{

    public partial class EvaluationRequestListView
    {
        public EvaluationRequestListView(IEventAggregator eventAggregator)
        {
            InitializeComponent();

            eventAggregator.GetEvent<OpenInquiryPopupEvent>().SubscribeAsync(async (nothing) =>
            {
                navigationDrawer.ToggleDrawer();
            }, keepSubscriberReferenceAlive: true, threadOption: ThreadOption.UIThread);
        }
    }
}

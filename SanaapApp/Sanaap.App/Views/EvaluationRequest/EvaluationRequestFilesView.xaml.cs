using Prism.Events;
using Sanaap.App.Events;

namespace Sanaap.App.Views.EvaluationRequest
{
    public partial class EvaluationRequestFilesView
    {
        public EvaluationRequestFilesView(IEventAggregator eventAggregator)
        {
            InitializeComponent();

            eventAggregator.GetEvent<TakePhotoEvent>().SubscribeAsync(async (request) =>
            {
                navigationDrawer.ToggleDrawer();
            }, keepSubscriberReferenceAlive: true, threadOption: ThreadOption.UIThread);
        }
    }
}

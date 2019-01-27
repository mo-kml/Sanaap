using Prism.Events;
using Rg.Plugins.Popup.Pages;
using Sanaap.App.Events;

namespace Sanaap.App.Views
{
    public partial class EvaluationRequestFilesView : PopupPage
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

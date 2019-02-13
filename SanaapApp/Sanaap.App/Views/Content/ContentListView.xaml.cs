using Prism.Events;
using Sanaap.App.Events;

using Xamarin.Forms;

namespace Sanaap.App.Views.Content
{

    public partial class ContentListView : ContentPage
    {
        public ContentListView(IEventAggregator eventAggregator)
        {
            InitializeComponent();

            eventAggregator.GetEvent<OpenNewsFilterPopupEvent>().SubscribeAsync(async (nothing) =>
            {
                navigationDrawer.ToggleDrawer();
            }, keepSubscriberReferenceAlive: true, threadOption: ThreadOption.UIThread);
        }
    }
}

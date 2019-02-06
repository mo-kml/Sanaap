using Prism.Events;
using Sanaap.App.Events;

using Xamarin.Forms;

namespace Sanaap.App.Views.Comment
{

    public partial class CommentListView : ContentPage
    {
        public CommentListView(IEventAggregator eventAggregator)
        {
            InitializeComponent();

            eventAggregator.GetEvent<OpenCreateCommentPopupEvent>().SubscribeAsync(async (nothing) =>
            {
                navigationDrawer.ToggleDrawer();
            }, keepSubscriberReferenceAlive: true, threadOption: ThreadOption.UIThread);
        }
    }
}

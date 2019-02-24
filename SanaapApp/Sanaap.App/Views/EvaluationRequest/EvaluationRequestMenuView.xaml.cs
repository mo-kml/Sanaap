using Prism.Events;
using Sanaap.App.Events;
using System;
using Xamarin.Forms;

namespace Sanaap.App.Views.EvaluationRequest
{

    public partial class EvaluationRequestMenuView : ContentPage, IDisposable
    {
        private SubscriptionToken SubscriptionToken;
        private ViewCell lastCell;
        public EvaluationRequestMenuView(IEventAggregator eventAggregator)
        {
            InitializeComponent();

            SubscriptionToken = eventAggregator.GetEvent<OpenInsurancePopupEvent>().SubscribeAsync(async (nothing) =>
              {
                  navigationDrawer.ToggleDrawer();
              }, keepSubscriberReferenceAlive: true, threadOption: ThreadOption.UIThread);
        }

        public void Dispose()
        {
            SubscriptionToken.Dispose();
        }

        private void ViewCell_Tapped(object sender, EventArgs e)
        {
            if (lastCell != null)
            {
                lastCell.View.BackgroundColor = Color.Transparent;
            }

            ViewCell viewCell = (ViewCell)sender;
            if (viewCell.View != null)
            {
                viewCell.View.BackgroundColor = (Color)Application.Current.Resources["primaryBlue"];
                lastCell = viewCell;
            }
        }
    }
}

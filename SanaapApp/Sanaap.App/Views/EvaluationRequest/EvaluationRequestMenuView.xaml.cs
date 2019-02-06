using Prism.Events;
using Sanaap.App.Events;
using System;
using Xamarin.Forms;

namespace Sanaap.App.Views.EvaluationRequest
{

    public partial class EvaluationRequestMenuView : ContentPage, IDisposable
    {
        private SubscriptionToken SubscriptionToken;
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
    }
}

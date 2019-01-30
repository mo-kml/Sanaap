using Prism.Events;
using Sanaap.App.Events;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Sanaap.App.Views.EvaluationRequest
{
    
    public partial class EvaluationRequestMenuView : ContentPage, IDisposable
    {
        private IEventAggregator _eventAggregator;
        private SubscriptionToken SubscriptionToken;
        public EvaluationRequestMenuView(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;

            InitializeComponent();

            SubscriptionToken = _eventAggregator.GetEvent<OpenInsurancePopupEvent>().SubscribeAsync(async (nothing) =>
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

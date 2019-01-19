using Prism.Events;
using Sanaap.App.Events;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Sanaap.App.Views.EvaluationRequest
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EvaluationRequestView : ContentPage
    {
        public EvaluationRequestView(IEventAggregator eventAggregator)
        {
            InitializeComponent();

            eventAggregator.GetEvent<OpenInsurancePopupEvent>().SubscribeAsync(async (nothing) =>
            {
                navigationDrawer.ToggleDrawer();
            });
        }
    }
}

using Prism.Events;

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
        }
    }
}

using Rg.Plugins.Popup.Pages;

namespace Sanaap.App.Views.EvaluationRequest
{
    public partial class EvaluationRequestDescriptionView : PopupPage
    {
        public EvaluationRequestDescriptionView()
        {
            InitializeComponent();
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}

using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Sanaap.App.Views.EvaluationRequest
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EvaluationRequestListView : ContentPage
    {
        public EvaluationRequestListView()
        {
            InitializeComponent();
        }

        private void InquiryDocument(object sender, EventArgs e)
        {
            navigationDrawer.ToggleDrawer();
        }
    }
}

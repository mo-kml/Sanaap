using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Sanaap.App.Views.EvaluationRequest
{
    
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

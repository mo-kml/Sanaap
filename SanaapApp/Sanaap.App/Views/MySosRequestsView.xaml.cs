using Sanaap.App.ViewModels;
using Xamarin.Forms;

namespace Sanaap.App.Views
{
    public partial class MySosRequestsView : ContentPage
    {
        public MySosRequestsView()
        {
            InitializeComponent();
        }
        protected override bool OnBackButtonPressed()
        {
            ((MySosRequestsViewModel)BindingContext).GoBack.Execute();
            return true;
        }
    }
}

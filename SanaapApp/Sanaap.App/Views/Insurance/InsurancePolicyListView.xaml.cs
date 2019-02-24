using System;

using Xamarin.Forms;

namespace Sanaap.App.Views.Insurance
{

    public partial class InsurancePolicyListView : ContentPage
    {
        private ViewCell lastCell;
        public InsurancePolicyListView()
        {
            InitializeComponent();
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

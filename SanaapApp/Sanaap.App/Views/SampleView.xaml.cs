using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Sanaap.App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SampleView : ContentPage
    {
        public SampleView()
        {
            InitializeComponent();
        }

        private void ClickToShowPopup_Clicked(object sender, EventArgs e)
        {
            //popupLayout.Show();
        }
    }
}

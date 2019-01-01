using Syncfusion.XForms.ComboBox;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Sanaap.App.Views.Insurance
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateInsurancePolicyView : ContentPage
    {
        public CreateInsurancePolicyView()
        {
            InitializeComponent();
        }

        private void SfComboBox_FilterCollectionChanged(object sender, Syncfusion.XForms.ComboBox.FilterCollectionChangedEventArgs e)
        {

        }

        private void SfComboBox_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            SfComboBox comboBox = ((SfComboBox)sender);

            if (e.PropertyName == nameof(comboBox.Text))
            {
                string a = comboBox.Text;
            }
        }

        private void SfComboBox_ValueChanged(object sender, Syncfusion.XForms.ComboBox.ValueChangedEventArgs e)
        {

        }

    }
}

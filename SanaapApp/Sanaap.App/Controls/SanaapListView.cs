using Xamarin.Forms;

namespace Sanaap.App.Controls
{
    public class SanaapListView : ListView
    {
        public static BindableProperty SelectedColorProperty = BindableProperty.Create(nameof(SelectedColor), typeof(Color), typeof(SanaapEntry), Color.FromHex("#bfbfbf"), BindingMode.TwoWay);
        public Color SelectedColor
        {
            get => (Color)GetValue(SelectedColorProperty);
            set => SetValue(SelectedColorProperty, value);
        }
    }
}

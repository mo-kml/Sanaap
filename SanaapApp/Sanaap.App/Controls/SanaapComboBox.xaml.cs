using System.Collections;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Sanaap.App.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SanaapComboBox : ContentView
    {
        public SanaapComboBox()
        {
            InitializeComponent();
        }

        private void ToggleMenu(object sender, System.EventArgs e)
        {
            //combobox.IsDropDownOpen = true;
        }

        public static readonly BindableProperty BorderColorProperty = BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(SanaapComboBox), Color.FromHex("#e2e2e2"), BindingMode.TwoWay);
        public Color BorderColor
        {
            get => (Color)GetValue(BorderColorProperty);
            set => SetValue(BorderColorProperty, value);
        }

        public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(SanaapComboBox), null, BindingMode.TwoWay);
        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static readonly BindableProperty FontFamilyProperty = BindableProperty.Create(nameof(FontFamily), typeof(string), typeof(SanaapComboBox), null, BindingMode.TwoWay);
        public string FontFamily
        {
            get => (string)GetValue(FontFamilyProperty);
            set => SetValue(FontFamilyProperty, value);
        }

        public static readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(SanaapComboBox), Color.Black, BindingMode.TwoWay);
        public Color TextColor
        {
            get => (Color)GetValue(TextColorProperty);
            set => SetValue(TextColorProperty, value);
        }

        public static readonly BindableProperty FontSizeProperty = BindableProperty.Create(nameof(FontSize), typeof(int), typeof(SanaapComboBox), 15, BindingMode.TwoWay);
        public int FontSize
        {

            get => (int)GetValue(FontSizeProperty);
            set => SetValue(FontSizeProperty, value);
        }

        public static readonly BindableProperty DataSourceProperty = BindableProperty.Create(nameof(DataSource), typeof(IEnumerable), typeof(SanaapComboBox), null, BindingMode.TwoWay);
        public IEnumerable DataSource
        {
            get => (IEnumerable)GetValue(DataSourceProperty);
            set => SetValue(DataSourceProperty, value);
        }

        public static readonly BindableProperty AllowFilteringProperty = BindableProperty.Create(nameof(AllowFiltering), typeof(bool), typeof(SanaapComboBox), false, BindingMode.TwoWay);
        public bool AllowFiltering
        {
            get => (bool)GetValue(AllowFilteringProperty);
            set => SetValue(AllowFilteringProperty, value);
        }

        public static readonly BindableProperty IsEditableModeProperty = BindableProperty.Create(nameof(IsEditableMode), typeof(bool), typeof(SanaapComboBox), false, BindingMode.TwoWay);
        public bool IsEditableMode
        {
            get => (bool)GetValue(IsEditableModeProperty);
            set => SetValue(IsEditableModeProperty, value);
        }

        public static readonly BindableProperty DisplayMemberPathProperty = BindableProperty.Create(nameof(DisplayMemberPath), typeof(string), typeof(SanaapComboBox), string.Empty, BindingMode.TwoWay);
        public string DisplayMemberPath
        {
            get => (string)GetValue(DisplayMemberPathProperty);
            set => SetValue(DisplayMemberPathProperty, value);
        }

        public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create(nameof(SelectedItem), typeof(object), typeof(SanaapComboBox), null, BindingMode.TwoWay);
        public object SelectedItem
        {
            get => GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }
    }
}


using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Sanaap.App.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SanaapEntry : ContentView
    {
        public SanaapEntry()
        {
            InitializeComponent();
        }

        public Color BorderColor
        {
            get => (Color)GetValue(BorderColorProperty);
            set => SetValue(BorderColorProperty, value);
        }

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public string FontFamily
        {
            get => (string)GetValue(FontFamilyProperty);
            set => SetValue(FontFamilyProperty, value);
        }

        public string Placeholder
        {
            get => (string)GetValue(PlaceholderProperty);
            set => SetValue(PlaceholderProperty, value);
        }

        public Color PlaceholderColor
        {
            get => (Color)GetValue(PlaceholderColorProperty);
            set => SetValue(PlaceholderColorProperty, value);
        }

        public int MaxLength
        {
            get => (int)GetValue(MaxLengthProperty);
            set => SetValue(MaxLengthProperty, value);
        }

        public Keyboard Keyboard
        {
            get => (Keyboard)GetValue(KeyboardProperty);
            set => SetValue(KeyboardProperty, value);
        }

        public Color TextColor
        {
            get => (Color)GetValue(TextColorProperty);
            set => SetValue(TextColorProperty, value);
        }

        public int FontSize
        {
            get => (int)GetValue(FontSizeProperty);
            set => SetValue(FontSizeProperty, value);
        }

        public static readonly BindableProperty TextProperty =
                BindableProperty.Create(nameof(Text), typeof(string), typeof(string), null);


        public static readonly BindableProperty FontFamilyProperty =
                BindableProperty.Create(nameof(FontFamily), typeof(string), typeof(string), null);

        public static readonly BindableProperty PlaceholderProperty =
                BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(string), null);


        public static readonly BindableProperty BorderColorProperty =
                BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(Color), Color.FromHex("#e2e2e2"));

        public static readonly BindableProperty PlaceholderColorProperty =
                BindableProperty.Create(nameof(PlaceholderColor), typeof(Color), typeof(Color), Color.FromHex("#e2e2e2"));

        public static readonly BindableProperty MaxLengthProperty =
                BindableProperty.Create(nameof(MaxLength), typeof(int), typeof(int), int.MaxValue);

        public static readonly BindableProperty KeyboardProperty =
                BindableProperty.Create(nameof(Keyboard), typeof(Keyboard), typeof(Keyboard), Keyboard.Default);

        public static readonly BindableProperty TextColorProperty =
                BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(Color), Color.Black);

        public static readonly BindableProperty FontSizeProperty =
                BindableProperty.Create(nameof(FontSize), typeof(int), typeof(int), 15);
    }
}

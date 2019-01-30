using Xamarin.Forms;

namespace Sanaap.App.Controls
{
    public class SanaapFrame : Frame
    {
        public static BindableProperty ShadowOffsetProperty = BindableProperty.Create(nameof(ShadowOffset), typeof(float), typeof(SanaapFrame), null, BindingMode.TwoWay);
        public float ShadowOffset
        {
            get => (float)GetValue(ShadowOffsetProperty);
            set => SetValue(ShadowOffsetProperty, value);
        }

        public static BindableProperty ShadowColorProperty = BindableProperty.Create(nameof(ShadowColor), typeof(Color), typeof(SanaapFrame), null, BindingMode.TwoWay);
        public Color ShadowColor
        {
            get => (Color)GetValue(ShadowColorProperty);
            set => SetValue(ShadowColorProperty, value);
        }
    }
}

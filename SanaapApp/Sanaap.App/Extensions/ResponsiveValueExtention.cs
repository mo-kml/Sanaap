using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Sanaap.App.Extensions
{
    [ContentProperty(nameof(Value))]
    /// <summary>
    /// You design based on 768*1280. We scale values on other devices!
    /// </summary>
    public class ResponsiveValueExtension : IMarkupExtension
    {
        // Height size of main device (Unit).
        public static int constHeight = 1280;

        // Width size of main device (Unit).
        public static int constWidth = 768;

        private static readonly ThicknessTypeConverter thicknessTypeConverter = new ThicknessTypeConverter();

        protected readonly double screenHeight = DeviceDisplay.MainDisplayInfo.Height;
        protected readonly double screenWidth = DeviceDisplay.MainDisplayInfo.Width;
        protected readonly double screenDensity = DeviceDisplay.MainDisplayInfo.Density;

        public object Value { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Value == null)
            {
                return null;
            }

            IProvideValueTarget provideValueTarget = serviceProvider.GetService<IProvideValueTarget>();

            if (provideValueTarget.TargetProperty is BindableProperty prop)
            {
                return Convert(prop);
            }
            else if (provideValueTarget.TargetObject is Setter setter)
            {
                return Convert(setter.Property);
            }

            return Value;
        }

        public object Convert(BindableProperty prop)
        {
            if (prop.ReturnType == typeof(Thickness))
            {
                Thickness valueThickness = (Thickness)thicknessTypeConverter.ConvertFromInvariantString(Value.ToString());
                return new Thickness((valueThickness.Left * (screenWidth / screenDensity)) / constWidth,
                (valueThickness.Top * (screenWidth / screenDensity)) / constWidth,
                (valueThickness.Right * (screenWidth / screenDensity)) / constWidth,
                (valueThickness.Bottom * (screenWidth / screenDensity)) / constWidth);
            }
            else if (prop.PropertyName.Contains(nameof(VisualElement.Width)))
            {
                double widthVal = double.Parse(Value.ToString());

                if (prop.ReturnType == typeof(GridLength))
                {
                    return new GridLength((widthVal * (screenWidth / screenDensity)) / constWidth);
                }
                else
                {
                    return (widthVal * (screenWidth / screenDensity)) / constWidth;
                }
            }
            else if (prop.PropertyName.Contains(nameof(VisualElement.Height)))
            {
                double heightVal = double.Parse(Value.ToString());

                if (prop.ReturnType == typeof(GridLength))
                {
                    return new GridLength((heightVal * (screenHeight / screenDensity)) / constHeight);
                }
                else
                {
                    return (heightVal * (screenHeight / screenDensity)) / constHeight;
                }
            }
            else if (prop.PropertyName == nameof(Grid.RowSpacing)
                || prop.PropertyName == nameof(Grid.ColumnSpacing)
                || prop.PropertyName == nameof(StackLayout.Spacing)
                || prop.PropertyName == nameof(Label.FontSize))
            {
                double spacingVal = double.Parse(Value.ToString());

                return (spacingVal * (screenWidth / screenDensity)) / constWidth;
            }

            return Value;
        }
    }
}

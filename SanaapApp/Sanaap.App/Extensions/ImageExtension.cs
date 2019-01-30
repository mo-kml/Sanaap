using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Sanaap.App.Extensions
{
    public class ImageExtension : BindableObject, IMarkupExtension
    {
        public static BindableProperty FileNameProperty = BindableProperty.Create(nameof(FileName), typeof(string), typeof(ImageExtension), null, BindingMode.TwoWay);
        public string FileName
        {
            get => (string)GetValue(FileNameProperty);
            set => SetValue(FileNameProperty, value);
        }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (FileName == null)
            {
                return null;
            }

            return ImageSource.FromResource($"Sanaap.App.Images.{FileName}");
        }
    }
}

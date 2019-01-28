using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Sanaap.App.Extensions
{
    public class ImageExtension : IMarkupExtension
    {
        public string Source { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Source == null)
            {
                return null;
            }

            return ImageSource.FromResource($"Sanaap.App.Images.{Source}");
        }
    }
}

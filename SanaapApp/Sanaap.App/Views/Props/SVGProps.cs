using SVG.Forms.Plugin.Abstractions;
using System.Reflection;
using Xamarin.Forms;

namespace Sanaap.App.Views.Props
{
    public static class SVGProps
    {
        public static readonly BindableProperty FileNameProperty =
            BindableProperty.CreateAttached(propertyName: "FileName", returnType: typeof(string), declaringType: typeof(SvgImage), defaultValue: null, propertyChanged: (sender, oldValue, newValue) =>
            {
                SvgImage image = (SvgImage)sender;

                image.SvgAssembly = typeof(SVGProps).GetTypeInfo().Assembly;

                image.SvgPath = $"Sanaap.App.Images.{newValue}";
            });

        public static string GetFileName(BindableObject view)
        {
            return (string)view.GetValue(FileNameProperty);
        }

        public static void SetFileName(BindableObject view, bool value)
        {
            view.SetValue(FileNameProperty, value);
        }
    }
}

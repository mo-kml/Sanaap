using Xamarin.Forms;

namespace Sanaap.App.Controls
{
    public enum FontAwesomeType
    {
        Regular,
        Solid,
        Brands
    }
    public class FontFamilies
    {
        public static string FontFamily(FontAwesomeType Type)
        {
            if (Device.RuntimePlatform == Device.Android)
            {
                switch (Type)
                {
                    case FontAwesomeType.Regular:
                        return "fa-regular-400.ttf#Font Awesome 5 Free Regular";
                    case FontAwesomeType.Solid:
                        return "fa-solid-900.ttf#Font Awesome 5 Free Solid";
                    case FontAwesomeType.Brands:
                        return "fa-brands-400.ttf#Font Awesome 5 Free Regular";
                    default:
                        break;
                }
            }
            else if (Device.RuntimePlatform == Device.iOS)
            {
                return "Font Awesome 5 Free";
            }
            return "";
        }
    }
}

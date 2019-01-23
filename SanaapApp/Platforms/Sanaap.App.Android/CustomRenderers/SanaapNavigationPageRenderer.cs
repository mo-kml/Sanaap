using Android.Content;
using Xamarin.Forms.Platform.Android.AppCompat;

namespace Sanaap.App.Droid.CustomRenderers
{
    public class SanaapNavigationPageRenderer : NavigationPageRenderer
    {
        public SanaapNavigationPageRenderer(Context context) : base(context)
        {
        }

        protected override void OnDetachedFromWindow()
        {
            if (Element == null)
            {
                return;
            }

            base.OnDetachedFromWindow();
        }
    }
}

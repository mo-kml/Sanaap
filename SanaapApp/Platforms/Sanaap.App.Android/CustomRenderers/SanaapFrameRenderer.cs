using Android.Content;
using Sanaap.App.Controls;
using Sanaap.App.Droid.CustomRenderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(SanaapFrame), typeof(SanaapFrameRenderer))]
namespace Sanaap.App.Droid.CustomRenderers
{
    public class SanaapFrameRenderer : Xamarin.Forms.Platform.Android.AppCompat.FrameRenderer
    {
        private SanaapFrame frame;
        public SanaapFrameRenderer(Context context) : base(context)
        {
        }
        protected override void OnElementChanged(ElementChangedEventArgs<Frame> e)
        {
            base.OnElementChanged(e);

            frame = Element as SanaapFrame;

            if (e.NewElement != null && frame != null)
            {
                Control.Elevation = frame.ShadowOffset;
            }
        }
    }
}

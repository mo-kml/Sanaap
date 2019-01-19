using Android.Content;
using Sanaap.App.Droid.CustomRenderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Entry), typeof(SanaapEntryRenderer))]
namespace Sanaap.App.Droid.CustomRenderers
{
    public class SanaapEntryRenderer : EntryRenderer
    {
        public SanaapEntryRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement == null)
            {
                Control.Background = null;

                int padding = (Control.CompoundPaddingBottom + Control.CompoundPaddingEnd) / 2;

                Control.SetPadding(padding, padding, padding, padding);
            }
        }
    }
}

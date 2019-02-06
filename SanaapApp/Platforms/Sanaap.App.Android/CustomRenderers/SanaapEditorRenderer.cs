using Android.Content;
using Sanaap.App.Droid.CustomRenderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Editor), typeof(SanaapEditorRenderer))]
namespace Sanaap.App.Droid.CustomRenderers
{
    public class SanaapEditorRenderer : EditorRenderer
    {
        public SanaapEditorRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement == null)
            {
                Control.Background = null;

                int padding = (Control.CompoundPaddingBottom + Control.CompoundPaddingEnd) / 2;

                Control.SetPadding(padding, 0, padding, 0);
            }
        }
    }
}

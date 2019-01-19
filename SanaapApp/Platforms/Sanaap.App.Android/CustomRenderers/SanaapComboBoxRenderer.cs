using Android.Content;
using Sanaap.App.Droid.CustomRenderers;
using Syncfusion.XForms.ComboBox;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(SfComboBox), typeof(SanaapComboBoxRenderer))]
namespace Sanaap.App.Droid.CustomRenderers
{
    public class SanaapComboBoxRenderer : ViewRenderer<SfComboBox, Syncfusion.Android.ComboBox.SfComboBox>
    {
        public SanaapComboBoxRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<SfComboBox> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement == null)
            {



            }
        }
    }
}

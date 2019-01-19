using Syncfusion.XForms.Android.ComboBox;
using Syncfusion.XForms.ComboBox;
using Xamarin.Forms.Platform.Android;

namespace Sanaap.App.Droid.CustomRenderers
{
    public class SanaapComboBoxRenderer : SfComboBoxRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<SfComboBox> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement == null)
            {
                Control.LayoutDirection = Android.Views.LayoutDirection.Rtl;
                //Control.DropDownButtonSettings.View.TextDirection = Android.Views.TextDirection.Ltr;
            }
        }
    }
}

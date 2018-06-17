using Xamarin.Forms;

namespace SanaapOperatorApp.Views.Props
{
    public class VisualElementProps
    {
        public static readonly BindableProperty IsFocusedProperty =
              BindableProperty.CreateAttached(propertyName: "IsFocused", returnType: typeof(bool), declaringType: typeof(VisualElement), defaultValue: false, propertyChanged: (sender, oldValue, newValue) =>
              {
                  if (newValue is bool newValueBool && newValueBool == true && sender is VisualElement visualElement)
                      visualElement.Focus();
              });

        public static bool GetIsFocused(BindableObject view)
        {
            return (bool)view.GetValue(IsFocusedProperty);
        }

        public static void SetIsFocused(BindableObject view, bool value)
        {
            view.SetValue(IsFocusedProperty, value);
        }
    }
}

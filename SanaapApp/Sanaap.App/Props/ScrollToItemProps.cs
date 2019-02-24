using Sanaap.App.ItemSources;
using Syncfusion.ListView.XForms;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace Sanaap.App.Props
{
    public static class ScrollToItemProps
    {
        public static readonly BindableProperty ScrollToItemProperty =
              BindableProperty.CreateAttached(propertyName: "ScrollToItem", returnType: typeof(object), declaringType: typeof(SfListView), defaultValue: null, propertyChanged: (sender, oldValue, newValue) =>
              {
                  SfListView listView = (SfListView)sender;

                  if (newValue != null)
                  {
                      int selectedItemIndex = listView.DataSource.DisplayItems.IndexOf(newValue);

                      (listView.LayoutManager as LinearLayout).ScrollToRowIndex(selectedItemIndex);
                  }
              });

        public static string GetScrollToItem(BindableObject view)
        {
            return (string)view.GetValue(ScrollToItemProperty);
        }

        public static void SetScrollToItem(BindableObject view, object value)
        {
            view.SetValue(ScrollToItemProperty, value);
        }

        public static readonly BindableProperty ScrollToLastItemProperty =
              BindableProperty.CreateAttached(propertyName: "ScrollToLastItem", returnType: typeof(object), declaringType: typeof(SfListView), defaultValue: null, propertyChanged: (sender, oldValue, newValue) =>
              {
                  SfListView listView = (SfListView)sender;

                  if (newValue != null && newValue is IEnumerable<InsurersItemSource> insurers)
                  {
                      int lastIndex = insurers.Count() - 1;

                      (listView.LayoutManager as LinearLayout).ScrollToRowIndex(lastIndex);
                  }
              });

        public static string GetScrollToLastItem(BindableObject view)
        {
            return (string)view.GetValue(ScrollToLastItemProperty);
        }

        public static void SetScrollToLastItem(BindableObject view, object value)
        {
            view.SetValue(ScrollToLastItemProperty, value);
        }
    }
}

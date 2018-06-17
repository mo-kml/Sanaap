using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace Sanaap.App.Views.Props
{
    public static class MapProps
    {
        public static readonly BindableProperty CenterRegionProperty =
              BindableProperty.CreateAttached(propertyName: "CenterRegion", returnType: typeof(Plugin.Geolocator.Abstractions.Position), declaringType: typeof(Map), defaultValue: null, propertyChanged: (sender, oldValue, newValue) =>
              {
                  Map map = (Map)sender;

                  if (newValue is Plugin.Geolocator.Abstractions.Position position)
                  {
                      map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(position.Latitude, position.Longitude), Distance.FromMeters(200)));
                  }
              });

        public static string GetCenterRegion(BindableObject view)
        {
            return (string)view.GetValue(CenterRegionProperty);
        }

        public static void SetCenterRegion(BindableObject view, Plugin.Geolocator.Abstractions.Position value)
        {
            view.SetValue(CenterRegionProperty, value);
        }
    }
}

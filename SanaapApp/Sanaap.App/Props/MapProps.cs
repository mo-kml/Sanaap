using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace Sanaap.App.Props
{
    public static class MapProps
    {
        public static readonly BindableProperty CenterRegionProperty =
              BindableProperty.CreateAttached(propertyName: "CenterRegion", returnType: typeof(Location), declaringType: typeof(Xamarin.Forms.GoogleMaps.Map), defaultValue: null, propertyChanged: (sender, oldValue, newValue) =>
              {
                  Xamarin.Forms.GoogleMaps.Map map = (Xamarin.Forms.GoogleMaps.Map)sender;

                  if (newValue is Location position)
                  {
                      map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(position.Latitude, position.Longitude), Distance.FromMeters(200)));
                  }
              });

        public static string GetCenterRegion(BindableObject view)
        {
            return (string)view.GetValue(CenterRegionProperty);
        }

        public static void SetCenterRegion(BindableObject view, Location value)
        {
            view.SetValue(CenterRegionProperty, value);
        }


        public static readonly BindableProperty UpdateToCenterRegionProperty =
              BindableProperty.CreateAttached(propertyName: "UpdateToCenterRegion", returnType: typeof(Location), declaringType: typeof(Xamarin.Forms.GoogleMaps.Map), defaultValue: null, propertyChanged: (sender, oldValue, newValue) =>
              {
                  Xamarin.Forms.GoogleMaps.Map map = (Xamarin.Forms.GoogleMaps.Map)sender;

                  if (newValue is Location position)
                  {
                      map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(position.Latitude, position.Longitude), Distance.FromMeters(200)));

                      map.Pins.Clear();

                      map.Pins.Add(new Pin
                      {
                          Icon = BitmapDescriptorFactory.FromBundle("carPin.png"),
                          Label = "",
                          Position = new Position(position.Latitude, position.Longitude)
                      });
                  }
              });

        public static string GetUpdateToCenterRegion(BindableObject view)
        {
            return (string)view.GetValue(UpdateToCenterRegionProperty);
        }

        public static void SetUpdateToCenterRegion(BindableObject view, Location value)
        {
            view.SetValue(UpdateToCenterRegionProperty, value);
        }
    }
}

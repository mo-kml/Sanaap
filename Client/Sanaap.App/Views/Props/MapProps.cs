using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Sanaap.App.Views.Props
{
    public class MapProps
    {
        public static readonly BindableProperty CurrentPositionProperty =
            BindableProperty.CreateAttached(propertyName: "CurrentPosition", returnType: typeof(Plugin.Geolocator.Abstractions.Position), declaringType: typeof(Map), defaultValue: null,
                propertyChanged: (sender, oldValue, newValue) =>
                {
                    Map map = (Map)sender;

                    if (newValue is Plugin.Geolocator.Abstractions.Position position)
                    {
                        map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(position.Latitude, position.Longitude), Distance.FromMeters(100)));
                    }
                });
    }
}

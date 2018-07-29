using System.Threading.Tasks;

namespace Xamarin.Essentials
{
    public static class GeolocationExtensions
    {
        public static Task<Location> GetLocation()
        {
            return Task.Run(() => Geolocation.GetLocationAsync().GetAwaiter().GetResult());
        }
    }
}

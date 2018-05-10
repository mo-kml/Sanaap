using Bit.ViewModel;
using Plugin.Geolocator.Abstractions;
using Prism.Navigation;

namespace Sanaap.App.ViewModels
{
    public class MainViewModel : BitViewModelBase
    {
        private readonly IGeolocator _geolocator;

        public MainViewModel(IGeolocator geolocator)
        {
            _geolocator = geolocator;
        }

        public virtual Position CurrentPosition { get; set; }

        public async override void OnNavigatedTo(NavigationParameters parameters)
        {
            if (_geolocator.IsGeolocationAvailable)
            {
                CurrentPosition = await _geolocator.GetPositionAsync();
            }

            base.OnNavigatedTo(parameters);
        }
    }
}

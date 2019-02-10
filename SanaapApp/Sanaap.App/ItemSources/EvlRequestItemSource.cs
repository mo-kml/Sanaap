using PropertyChanged;
using Sanaap.Dto;

namespace Sanaap.App.ItemSources
{
    [AddINotifyPropertyChangedInterface]
    public class EvlRequestItemSource : EvlRequestDto
    {
        public LicensePlateItemSource LicensePlateItemSource { get; set; }

        public LicensePlateItemSource LostLicensePlateItemSource { get; set; }
    }
}

using Sanaap.App.ItemSources;

namespace Sanaap.App.Helpers.Contracts
{
    public interface ILicenseHelper
    {
        bool ConvertToPlateNumber(LicensePlateItemSource licensePlateItemSource, out string licensePlate);

        LicensePlateItemSource ConvertToItemSource(string licensePlate);
    }
}

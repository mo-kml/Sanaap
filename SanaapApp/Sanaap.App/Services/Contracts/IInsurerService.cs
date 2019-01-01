using Sanaap.App.ItemSources;

namespace Sanaap.App.Services.Contracts
{
    public interface IInsurerService
    {
        InsurersItemSource[] GetAllInsurers();
    }
}

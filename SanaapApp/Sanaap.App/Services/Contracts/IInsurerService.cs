using Sanaap.App.ItemSources;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sanaap.App.Services.Contracts
{
    public interface IInsurerService
    {
        Task<List<InsurersItemSource>> GetAllInsurers();
    }
}

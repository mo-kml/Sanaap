using Sanaap.App.ItemSources;
using Sanaap.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sanaap.App.Services.Contracts
{
    public interface IInitialDataService
    {
        Task<List<ExternalEntityDto>> GetCars();

        Task<List<ExternalEntityDto>> GetColors();

        Task<CustomerDto> GetCurrentUserInfo();

        Task<List<InsurersItemSource>> GetInsurers();
    }
}

using Sanaap.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sanaap.Api.Contracts
{
    public interface IExternalApiService
    {
        Task<IEnumerable<ExternalEntityDto>> GetColors();

        Task<IEnumerable<ExternalEntityDto>> GetCars();

        Task<IEnumerable<InsurerDto>> GetInsurers();

    }
}

using Sanaap.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sanaap.App.Services.Contracts
{
    public interface IInitialDataService
    {
        Task<IEnumerable<ExternalEntityDto>> GetCars();

        Task<IEnumerable<ExternalEntityDto>> GetColors();
    }
}

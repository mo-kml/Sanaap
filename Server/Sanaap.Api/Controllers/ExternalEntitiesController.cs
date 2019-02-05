using Bit.OData.ODataControllers;
using Sanaap.Api.Contracts;
using Sanaap.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sanaap.Api.Controllers
{
    public class ExternalEntitiesController : DtoController<ExternalEntityDto>
    {
        public virtual IExternalApiService ExternalApiService { get; set; }

        [Function]
        public async Task<IEnumerable<ExternalEntityDto>> GetColors()
        {
            return await ExternalApiService.GetColors();
        }

        [Function]
        public async Task<IEnumerable<ExternalEntityDto>> GetCars()
        {
            return await ExternalApiService.GetCars();
        }

        [Function]
        public async Task<IEnumerable<ExternalEntityDto>> GetAlphabets()
        {
            return await ExternalApiService.GetNumberplateAlphabets();
        }

    }
}

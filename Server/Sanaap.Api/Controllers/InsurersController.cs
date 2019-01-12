using Bit.OData.ODataControllers;
using Sanaap.Api.Contracts;
using Sanaap.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sanaap.Api.Controllers
{
    public class InsurersController : DtoController<InsurerDto>
    {
        public virtual IExternalApiService ExternalApiService { get; set; }

        [Function]
        public async Task<IEnumerable<InsurerDto>> GetInsurers()
        {
            return await ExternalApiService.GetInsurers();
        }
    }
}

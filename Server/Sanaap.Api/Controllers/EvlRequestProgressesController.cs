using Bit.OData.ODataControllers;
using Sanaap.Api.Contracts;
using Sanaap.Dto;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Sanaap.Api.Controllers
{
    public class EvlRequestProgressesController : DtoController<EvlRequestProgressDto>
    {
        public virtual IExternalApiService ExternalApiService { get; set; }

        [Function]
        public virtual async Task<IEnumerable<EvlRequestProgressDto>> GetAllByRequestId(int fileId, CancellationToken cancellationToken)
        {
            return await ExternalApiService.GetFileProgress(fileId);
        }
    }
}

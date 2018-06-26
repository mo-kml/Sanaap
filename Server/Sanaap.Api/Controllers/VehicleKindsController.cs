using Bit.Model.Contracts;
using Sanaap.App.Dto;
using Sanaap.Model;
using Sannap.Data.Contracts;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace Sanaap.Api.Controllers
{
    [RoutePrefix("VehicleKinds")]
    public class VehicleKindsController : ApiController
    {
        public virtual ISanaapRepository<VehicleKind> VehicleKindRepository { get; set; }

        public virtual IDtoEntityMapper<VehicleKindDto, VehicleKind> DtoEntityMapper { get; set; }

        [HttpGet, Route("GetAll")]
        public virtual async Task<IQueryable<VehicleKindDto>> GetAll(CancellationToken cancellationToken)
        {
            return DtoEntityMapper.FromEntityQueryToDtoQuery(await VehicleKindRepository.GetAllAsync(cancellationToken));
        }
    }
}

using Bit.Model.Contracts;
using Bit.OData.ODataControllers;
using Sanaap.App.Dto;
using Sanaap.Data.Contracts;
using Sanaap.Model;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sanaap.Api.Controllers
{
    public class VehicleKindsController : DtoController<VehicleKindDto>
    {
        public virtual ISanaapRepository<VehicleKind> VehicleKindRepository { get; set; }

        public virtual IDtoEntityMapper<VehicleKindDto, VehicleKind> DtoEntityMapper { get; set; }

        [Get]
        public virtual async Task<IQueryable<VehicleKindDto>> GetAll(CancellationToken cancellationToken)
        {
            return DtoEntityMapper.FromEntityQueryToDtoQuery(await VehicleKindRepository.GetAllAsync(cancellationToken));
        }
    }
}

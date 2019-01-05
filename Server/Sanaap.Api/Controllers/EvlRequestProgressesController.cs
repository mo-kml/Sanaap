using Bit.Model.Contracts;
using Bit.OData.ODataControllers;
using Sanaap.Data.Contracts;
using Sanaap.Dto;
using Sanaap.Model;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sanaap.Api.Controllers
{
    public class EvlRequestProgressesController : SanaapDtoSetController<EvlRequestProgressDto, EvlRequestProgress>
    {
        public virtual ISanaapRepository<EvlRequestProgress> Repository { get; set; }

        public virtual IDtoEntityMapper<EvlRequestProgressDto, EvlRequestProgress> DtoEntityMapper { get; set; }

        [Function]
        public virtual async Task<IQueryable<EvlRequestProgressDto>> GetAllByRequestId(Guid requestId, CancellationToken cancellationToken)
        {
            return DtoEntityMapper.FromEntityQueryToDtoQuery((await Repository.GetAllAsync(cancellationToken)).Where(c => c.EvlRequestId == requestId));
        }

        public override async Task<IQueryable<EvlRequestProgressDto>> GetAll(CancellationToken cancellationToken)
        {
            return DtoEntityMapper.FromEntityQueryToDtoQuery((await Repository.GetAllAsync(cancellationToken)).Include(p => p.EvlRequest));
        }
    }
}

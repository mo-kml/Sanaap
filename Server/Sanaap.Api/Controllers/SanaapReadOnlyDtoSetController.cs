using System;
using System.IdentityModel;
using System.Threading;
using System.Threading.Tasks;
using System.Web.OData;
using Bit.Model.Contracts;

namespace Sanaap.Api.Controllers
{
    public class SanaapReadOnlyDtoSetController<TDto, TEntity> : SanaapDtoSetController<TDto, TEntity>
        where TDto : class, IDto
        where TEntity : class, IEntity
    {
        public override Task<TDto> Create(TDto dto, CancellationToken cancellationToken)
        {
            throw new BadRequestException("Change is denied");
        }

        public override Task<TDto> Update(Guid key, TDto dto, CancellationToken cancellationToken)
        {
            throw new BadRequestException("Change is denied");
        }

        public override Task<TDto> PartialUpdate(Guid key, Delta<TDto> modifiedDtoDelta, CancellationToken cancellationToken)
        {
            throw new BadRequestException("Change is denied");
        }

        public override Task Delete(Guid key, CancellationToken cancellationToken)
        {
            throw new BadRequestException("Change is denied");
        }
    }
}

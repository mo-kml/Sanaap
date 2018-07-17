using Bit.Model.Contracts;
using Bit.Owin.Exceptions;
using Microsoft.AspNet.OData;
using System;
using System.Threading;
using System.Threading.Tasks;

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

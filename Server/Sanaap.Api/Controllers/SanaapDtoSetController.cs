using Bit.Model.Contracts;
using Bit.OData.ODataControllers;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sanaap.Api.Controllers
{
    public class SanaapDtoSetController<TDto, TEntity> : DtoSetController<TDto, TEntity, Guid>
        where TDto : class, IDto
        where TEntity : class, IEntity
    {
        public override async Task<TDto> Create(TDto dto, CancellationToken cancellationToken)
        {
            return await base.Create(dto, cancellationToken);
        }

        public override async Task<TDto> Update(Guid key, TDto dto, CancellationToken cancellationToken)
        {
            return await base.Update(key, dto, cancellationToken);
        }

        public override async Task Delete(Guid key, CancellationToken cancellationToken)
        {
            await base.Delete(key, cancellationToken);
        }

        protected override async Task<TDto> GetById(Guid key, CancellationToken cancellationToken)
        {
            return await base.GetById(key, cancellationToken);
        }

        public override async Task<IQueryable<TDto>> GetAll(CancellationToken cancellationToken)
        {
            return await base.GetAll(cancellationToken);
        }
    }
}

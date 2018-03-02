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

        public async override Task<TDto> Update(Guid key, TDto dto, CancellationToken cancellationToken)
        {
            return await base.Update(key, dto, cancellationToken);
        }

        public async override Task Delete(Guid key, CancellationToken cancellationToken)
        {
            await base.Delete(key, cancellationToken);
        }

        protected async override Task<TDto> GetById(Guid key, CancellationToken cancellationToken)
        {
            return await base.GetById(key, cancellationToken);
        }

        public async override Task<IQueryable<TDto>> GetAll(CancellationToken cancellationToken)
        {
            return await base.GetAll(cancellationToken);
        }
    }
}

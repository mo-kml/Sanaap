using System.Threading;
using System.Threading.Tasks;
using Bit.Data.EntityFramework.Implementations;
using Bit.Model.Contracts;
using Sannap.Data.Contracts;

namespace Sannap.Data.Implementations
{
    public class SanaapEfRepository<TEntity> : EfRepository<TEntity>, ISanaapRepository<TEntity>
        where TEntity : class, IEntity
    {
        public async override Task<TEntity> AddAsync(TEntity entityToAdd, CancellationToken cancellationToken)
        {
            return await base.AddAsync(entityToAdd, cancellationToken);
        }
    }
}

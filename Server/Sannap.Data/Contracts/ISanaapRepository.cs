using Bit.Data.Contracts;
using Bit.Model.Contracts;

namespace Sannap.Data.Contracts
{
    public interface ISanaapRepository<TEntity> : IRepository<TEntity>
        where TEntity : class, IEntity
    {

    }
}

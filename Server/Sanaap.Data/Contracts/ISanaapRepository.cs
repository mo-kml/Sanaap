using Bit.Data.Contracts;
using Bit.Model.Contracts;

namespace Sanaap.Data.Contracts
{
    public interface ISanaapRepository<TEntity> : IRepository<TEntity>
        where TEntity : class, IEntity
    {

    }
}

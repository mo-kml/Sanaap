using Bit.Data.Contracts;
using Bit.Model.Contracts;
using System.Threading.Tasks;

namespace Sanaap.Data.Contracts
{
    public interface ISanaapRepository<TEntity> : IRepository<TEntity>
        where TEntity : class, IEntity
    {
        Task<int> GetNextSequenceValue();
    }
}

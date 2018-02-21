using Sanaap.Model;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sannap.Data.Contracts
{
    public interface IExpertsRepository : ISanaapRepository<Expert>
    {
        Task<IQueryable<Expert>> GetManExprts(CancellationToken cancellationToken);

        Task<IQueryable<Expert>> GetNearestExpert(CancellationToken cancellationToken);
    }
}

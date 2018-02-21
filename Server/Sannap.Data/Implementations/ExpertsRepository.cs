using Sanaap.Model;
using Sannap.Data.Contracts;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sannap.Data.Implementations
{
    public class ExpertsRepository : SanaapEfRepository<Expert>, IExpertsRepository
    {
        public virtual async Task<IQueryable<Expert>> GetManExprts(CancellationToken cancellationToken)
        {
            return (await GetAllAsync(cancellationToken))
                .Where(e => e.Gender == Gender.Man);
        }

        public virtual async Task<IQueryable<Expert>> GetNearestExpert(CancellationToken cancellationToken)
        {
            return (await GetAllAsync(cancellationToken)).Take(1);
        }
    }
}

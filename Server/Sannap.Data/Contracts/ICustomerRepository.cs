using Sanaap.Model;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sannap.Data.Contracts
{
    public interface ICustomersRepository : ISanaapRepository<Customer>
    {
        Task AddCustomer(Customer customer,CancellationToken cancellationToken);
    }
}

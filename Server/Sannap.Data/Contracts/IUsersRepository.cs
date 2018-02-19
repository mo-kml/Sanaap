using Sanaap.Model;
using System.Threading;
using System.Threading.Tasks;

namespace Sannap.Data.Contracts
{
    public interface IUsersRepository : ISanaapRepository<User>
    {
        Task<User> GetUserByUserNameAndPassword(string userName, string password, CancellationToken cancellationToken);
        Task<User> GetUserById(string userId, CancellationToken none);
    }
}

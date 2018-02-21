using Bit.Core.Contracts;
using Sanaap.Model;
using Sannap.Data.Contracts;
using System;
using System.Data.Entity;
using System.Threading;
using System.Threading.Tasks;

namespace Sannap.Data.Implementations
{
    public class UsersRepository : SanaapEfRepository<User>, IUsersRepository
    {
        public virtual async Task<User> GetUserById(Guid userId, CancellationToken cancellationToken)
        {
            return await GetByIdAsync(cancellationToken, userId);
        }

        public virtual async Task<User> GetUserByUserNameAndPassword(string userName, string password, CancellationToken cancellationToken)
        {
            if (userName == null)
                throw new ArgumentNullException(nameof(userName));

            if (password == null)
                throw new ArgumentNullException(nameof(password));

            userName = userName.ToLower();

            return await (await GetAllAsync(cancellationToken))
                .FirstOrDefaultAsync(u => u.UserName.ToLower() == userName && u.Password == password, cancellationToken);
        }
    }
}

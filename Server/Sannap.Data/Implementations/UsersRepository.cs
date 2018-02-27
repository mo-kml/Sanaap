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
        public virtual IHashService HashService { get; set; }

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

            User user = await (await GetAllAsync(cancellationToken))
                .SingleOrDefaultAsync(u => u.UserName.ToLower() == userName, cancellationToken);

            if (user == null)
                throw new InvalidOperationException("UserNotFound");

            HashService.VerifyHash(password, user.Password);

            return user;
        }
    }
}

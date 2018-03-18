using Bit.Core.Contracts;
using Bit.IdentityServer.Implementations;
using Bit.Owin.Exceptions;
using IdentityServer3.Core.Models;
using Sanaap.Model;
using Sannap.Data.Contracts;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Sanaap.Api.Implementations.Security
{
    public class SanaapUserService : UserService
    {
        public virtual IUsersRepository UsersRepository { get; set; }

        public override async Task<string> GetUserIdByLocalAuthenticationContextAsync(LocalAuthenticationContext context)
        {
            string username = context.UserName;
            string password = context.Password;

            if (string.IsNullOrEmpty(username))
                throw new ArgumentException(nameof(username));

            if (string.IsNullOrEmpty(password))
                throw new ArgumentException(nameof(password));

            User user = null;

            user = await UsersRepository.GetUserByUserNameAndPassword(context.UserName, context.Password, CancellationToken.None);

            if (user == null)
                throw new DomainLogicException("LoginFailed");

            return user.Id.ToString();
        }

        public override async Task<bool> UserIsActiveAsync(IsActiveContext context, string userId)
        {
            User user = await UsersRepository.GetUserById(Guid.Parse(userId), CancellationToken.None);

            return user != null;
        }
    }
}

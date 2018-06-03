﻿using Bit.IdentityServer.Implementations;
using Bit.Owin.Exceptions;
using IdentityServer3.Core.Models;
using Sanaap.Model;
using Sannap.Data.Contracts;
using System.Data.Entity;
using System.Threading;
using System.Threading.Tasks;

namespace Sanaap.Api.Implementations.Security
{
    public class SanaapUserService : UserService
    {
        public virtual ISanaapRepository<Customer> CustomersRepository { get; set; }

        public override async Task<string> GetUserIdByLocalAuthenticationContextAsync(LocalAuthenticationContext context, CancellationToken cancellationToken)
        {
            string username = context.UserName;
            string password = context.Password;

            if (string.IsNullOrEmpty(username))
                throw new BadRequestException("InvalidUserNameOrPassword");

            if (string.IsNullOrEmpty(password))
                throw new BadRequestException("InvalidUserNameOrPassword");

            string nationalCode = username;
            string mobile = password;

            Customer customer = await (await CustomersRepository
                .GetAllAsync(cancellationToken))
                .SingleOrDefaultAsync(c => c.Mobile == mobile && c.NationalCode == nationalCode);

            if (customer == null)
                throw new ResourceNotFoundException("CustomerCouldNotBeFound");

            return customer.Id.ToString();
        }

        public override async Task<bool> UserIsActiveAsync(IsActiveContext context, string userId, CancellationToken cancellationToken)
        {
            return true;
        }
    }
}

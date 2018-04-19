using Bit.IdentityServer.Implementations;
using Bit.Owin.Exceptions;
using IdentityServer3.Core.Models;
using Sanaap.Model;
using Sannap.Data.Contracts;
using System;
using System.Data.Entity;
using System.Threading;
using System.Threading.Tasks;

namespace Sanaap.Api.Implementations.Security
{
    public class SanaapUserService : UserService
    {
        public virtual ISanaapRepository<Customer> CustomersRepository { get; set; }

        public override async Task<string> GetUserIdByLocalAuthenticationContextAsync(LocalAuthenticationContext context)
        {
            string username = context.UserName;
            string password = context.Password;

            if (string.IsNullOrEmpty(username))
                throw new ArgumentException(nameof(username));

            if (string.IsNullOrEmpty(password))
                throw new ArgumentException(nameof(password));

            long nationalCode = long.Parse(username);

            if (password.Length == 4) // OTP, First login
            {
                int otp = int.Parse(password);

                Customer customer = await (await CustomersRepository
                    .GetAllAsync(CancellationToken.None))
                    .SingleOrDefaultAsync(c => c.OTP == otp && c.NationalCode == nationalCode);

                if (customer == null)
                    throw new ResourceNotFoundException("CustomerCouldNotBeFound");

                if (customer.IsActive == true)
                    throw new DomainLogicException("CustomerHasToUseMobileNumberAndNationalCodeForLogin");

                customer.IsActive = true;

                await CustomersRepository.UpdateAsync(customer, CancellationToken.None);

                return customer.Id.ToString();
            }
            else
            {
                long mobileNumber = long.Parse(password);

                Customer customer = await (await CustomersRepository
                    .GetAllAsync(CancellationToken.None))
                    .SingleOrDefaultAsync(c => c.Mobile == mobileNumber && c.NationalCode == nationalCode);

                if (customer == null)
                    throw new ResourceNotFoundException("CustomerCouldNotBeFound");

                return customer.Id.ToString();
            }
        }

        public override async Task<bool> UserIsActiveAsync(IsActiveContext context, string userId)
        {
            return true;
        }
    }
}

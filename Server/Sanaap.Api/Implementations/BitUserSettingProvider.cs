using Bit.Core.Contracts;
using Bit.Model.DomainModels;
using Bit.Owin.Contracts;
using Sanaap.Model;
using Sannap.Data.Contracts;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Sanaap.Api.Implementations
{
    public class SanaapUserSettingProvider : IUserSettingProvider
    {
        public virtual IUserInformationProvider UserInformationProvider { get; set; }
        public virtual ISanaapRepository<Customer> CustomerRepository { get; set; }

        public UserSetting GetCurrentUserSetting()
        {
            if (UserInformationProvider.IsAuthenticated())
            {
                Customer customer = CustomerRepository.GetById(Guid.Parse(UserInformationProvider.GetCurrentUserId()));

                UserSetting result = new UserSetting
                {
                    UserId = customer.Id.ToString(),
                };

                return result;
            }

            return null;
        }

        public async Task<UserSetting> GetCurrentUserSettingAsync(CancellationToken cancellationToken)
        {
            if (UserInformationProvider.IsAuthenticated())
            {
                Customer customer = await CustomerRepository.GetByIdAsync(cancellationToken, Guid.Parse(UserInformationProvider.GetCurrentUserId()));

                UserSetting result = new UserSetting
                {
                    UserId = customer.Id.ToString(),
                };

                return result;
            }

            return null;
        }
    }
}

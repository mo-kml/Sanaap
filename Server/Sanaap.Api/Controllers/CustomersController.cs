using Bit.Core.Contracts;
using Bit.Model.Contracts;
using Bit.OData.ODataControllers;
using Bit.Owin.Exceptions;
using Sanaap.Data.Contracts;
using Sanaap.Dto;
using Sanaap.Model;
using Sanaap.Service.Contracts;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace Sanaap.Api.Controllers
{
    public class CustomersController : DtoController<CustomerDto>
    {
        public virtual ICustomerValidator CustomerValidator { get; set; }

        public virtual ISanaapRepository<Customer> CustomersRepository { get; set; }

        public virtual IDtoEntityMapper<CustomerDto, Customer> DtoEntityMapper { get; set; }

        public virtual IUserInformationProvider UserInformationProvider { get; set; }

        public class RegisterCustomerArgs
        {
            public CustomerDto customer { get; set; }
        }

        [Action]
        [AllowAnonymous]
        public virtual async Task RegisterCustomer(RegisterCustomerArgs args, CancellationToken cancellationToken)
        {
            if (!CustomerValidator.IsValid(args.customer, out string errorMessage))
                throw new DomainLogicException(errorMessage);

            Customer customer = DtoEntityMapper.FromDtoToEntity(args.customer);

            bool existingCustomer = await (await CustomersRepository.GetAllAsync(cancellationToken))
                .Where(cu => cu.NationalCode == customer.NationalCode || cu.Mobile == customer.Mobile)
                .AnyAsync(cancellationToken);

            if (existingCustomer)
                throw new DomainLogicException("CustomerIsAlreadyRegistered");
            else
                DtoEntityMapper.FromEntityToDto(await CustomersRepository.AddAsync(customer, cancellationToken));
        }

        [Function]
        public virtual async Task<SingleResult<CustomerDto>> GetCurrentCustomer(CancellationToken cancellationToken)
        {
            Guid customerId = Guid.Parse(UserInformationProvider.GetCurrentUserId());

            return SingleResult.Create(DtoEntityMapper.FromEntityQueryToDtoQuery((await CustomersRepository.GetAllAsync(cancellationToken)).Where(c => c.Id == customerId)));
        }
    }
}

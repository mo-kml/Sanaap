using Bit.OData.ODataControllers;
using Sanaap.Dto;
using Sanaap.Model;
using Sannap.Data.Contracts;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Sanaap.Api.Controllers
{
    public class CustomersController : SanaapDtoSetController<CustomerDto, Customer>
    {
        public virtual ICustomersRepository CustomersRepository { get; set; }

        [Action]
        public virtual async Task AddCustomer(Customer customer, CancellationToken cancellation)
        {
            //CustomersRepository.GetAllAsync(cancellation).Where

            customer.OTP = GenerateRandomNo();
            await CustomersRepository.AddAsync(customer, cancellation);
        }

        private int GenerateRandomNo()
        {
            int min = 1000;
            int max = 9999;
            Random random = new Random();
            return random.Next(min, max);
        }
    }
}
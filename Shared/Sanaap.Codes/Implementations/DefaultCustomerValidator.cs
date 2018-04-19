using Sanaap.Dto;
using Sanaap.Service.Contracts;
using System;

namespace Sanaap.Service.Implementations
{
    public class DefaultCustomerValidator : ICustomerValidator
    {
        public bool IsValid(CustomerDto customer, out string message)
        {
            if (customer == null)
                throw new ArgumentNullException(nameof(customer));

            if (string.IsNullOrEmpty(customer.FirstName))
            {
                message = $"{nameof(CustomerDto.FirstName)}IsEmpty";
                return false;
            }

            if (string.IsNullOrEmpty(customer.LastName))
            {
                message = $"{nameof(CustomerDto.LastName)}IsEmpty";
                return false;
            }

            // ToDo: Validate MobileNumber & NationalCode here.

            message = null;
            return true;
        }
    }
}

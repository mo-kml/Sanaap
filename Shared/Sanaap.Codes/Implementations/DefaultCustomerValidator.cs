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

            if (string.IsNullOrEmpty(customer.NationalCode.ToString()))
            {
                message = $"{nameof(CustomerDto.NationalCode)}IsEmpty";
                return false;
            }

            if (customer.NationalCode.ToString().Length != 10)
            {
                message = $"{nameof(CustomerDto.NationalCode)}IsInvalid";
                return false;
            }

            if (string.IsNullOrEmpty(customer.Mobile.ToString()))
            {
                message = $"{nameof(CustomerDto.Mobile)}IsEmpty";
                return false;
            }

            if (customer.Mobile.ToString().Length != 11)
            {
                message = $"{nameof(CustomerDto.Mobile)}IsInvalid";
                return false;
            }

            if (customer.Mobile.ToString().Substring(0, 2) != "09")
            {
                message = $"{nameof(CustomerDto.Mobile)}IsInvalid";
                return false;
            }


            message = null;
            return true;
        }
    }
}

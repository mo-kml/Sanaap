using Sanaap.Dto;
using Sanaap.Service.Contracts;
using System;
using System.Linq;
using System.Text.RegularExpressions;

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

            if (customer.FirstName.Length < 3)
            {
                message = $"{nameof(CustomerDto.FirstName)}MustAtLeast3Character";
                return false;
            }

            if (string.IsNullOrEmpty(customer.LastName))
            {
                message = $"{nameof(CustomerDto.LastName)}IsEmpty";
                return false;
            }

            if (customer.LastName.Length < 3)
            {
                message = $"{nameof(CustomerDto.LastName)}MustAtLeast3Character";
                return false;
            }

            if (string.IsNullOrEmpty(customer.NationalCode))
            {
                message = $"{nameof(CustomerDto.NationalCode)}IsEmpty";
                return false;
            }

            customer.NationalCode = customer.NationalCode.Trim();

            if (!IsValidIranianNationalCode(customer.NationalCode))
            {
                message = $"{nameof(CustomerDto.NationalCode)}IsInvalid";
                return false;
            }

            if (string.IsNullOrEmpty(customer.Mobile))
            {
                message = $"{nameof(CustomerDto.Mobile)}IsEmpty";
                return false;
            }

            customer.Mobile = customer.Mobile.Trim();

            if (customer.Mobile.Length != 11)
            {
                message = $"{nameof(CustomerDto.Mobile)}IsInvalid";
                return false;
            }

            if (customer.Mobile.Substring(0, 2) != "09")
            {
                message = $"{nameof(CustomerDto.Mobile)}IsInvalid";
                return false;
            }

            message = null;

            return true;
        }

        public virtual bool IsValidIranianNationalCode(string input)
        {
            if (!Regex.IsMatch(input, @"^\d{10}$"))
                return false;

            int check = Convert.ToInt32(input.Substring(9, 1));

            int sum = Enumerable.Range(0, 9)
                .Select(x => Convert.ToInt32(input.Substring(x, 1)) * (10 - x))
                .Sum() % 11;

            return (sum < 2 && check == sum) || (sum >= 2 && check + sum == 11);
        }
    }
}

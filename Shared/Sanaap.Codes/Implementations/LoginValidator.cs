using Sanaap.Dto;
using Sanaap.Service.Contracts;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Sanaap.Service.Implementations
{
    public class LoginValidator : ILoginValidator
    {
        public bool IsValid(LoginDto loginDto, out string message)
        {
            if (loginDto.NationalCode == null)
            {
                message = $"{nameof(CustomerDto.NationalCode)}IsEmpty";
                return false;
            }
            loginDto.NationalCode = loginDto.NationalCode.Trim();
            if (string.IsNullOrEmpty(loginDto.NationalCode))
            {
                message = $"{nameof(CustomerDto.NationalCode)}IsEmpty";
                return false;
            }
            if (!IsValidIranianNationalCode(loginDto.NationalCode))
            {
                message = $"{nameof(CustomerDto.NationalCode)}IsInvalid";
                return false;
            }

            if (loginDto.Mobile == null)
            {
                message = $"{nameof(CustomerDto.Mobile)}IsEmpty";
                return false;
            }
            loginDto.Mobile = loginDto.Mobile.Trim();
            if (string.IsNullOrEmpty(loginDto.Mobile))
            {
                message = $"{nameof(CustomerDto.Mobile)}IsEmpty";
                return false;
            }
            if (loginDto.Mobile.Length != 11)
            {
                message = $"{nameof(CustomerDto.Mobile)}IsInvalid";
                return false;
            }
            if (loginDto.Mobile.Substring(0, 2) != "09")
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

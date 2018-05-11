using Sanaap.Dto;
using Sanaap.Service.Contracts;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Sanaap.Service.Implementations
{
    public class LoginValidator : ILoginValidator
    {
        public bool IsValid(string nationalCode, string mobile, out string message)
        {
            if (string.IsNullOrEmpty(nationalCode))
            {
                //message = $"{nameof(CustomerDto.NationalCode)}IsEmpty";
                message = "کد ملی را وارد نمائید";
                return false;
            }

            nationalCode = nationalCode.Trim();

            if (!IsValidIranianNationalCode(nationalCode))
            {
                //message = $"{nameof(CustomerDto.NationalCode)}IsInvalid";
                message = "کد ملی معتبر نیست";
                return false;
            }

            if (string.IsNullOrEmpty(mobile))
            {
                //message = $"{nameof(CustomerDto.Mobile)}IsEmpty";
                message = "موبایل را وارد نمائید";
                return false;
            }

            mobile = mobile.Trim();

            if (mobile.Length != 11 || mobile.Substring(0, 2) != "09")
            {
                //message = $"{nameof(CustomerDto.Mobile)}IsInvalid";
                message = "موبایل معتبر نیست";
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

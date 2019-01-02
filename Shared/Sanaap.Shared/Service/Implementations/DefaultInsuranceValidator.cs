using Sanaap.Dto;
using Sanaap.Service.Contracts;
using System;

namespace Sanaap.Service.Implementations
{
    public class DefaultInsuranceValidator : IInsuranceValidator
    {
        public bool IsValid(InsurancePolicyDto insurance, out string message)
        {
            if (insurance == null)
            {
                throw new ArgumentNullException(nameof(insurance));
            }

            if (string.IsNullOrEmpty(insurance.ChasisNo))
            {
                message = $"{nameof(InsurancePolicyDto.ChasisNo)}IsEmpty";
                return false;
            }

            if (string.IsNullOrEmpty(insurance.InsurerNo))
            {
                message = $"{nameof(InsurancePolicyDto.InsurerNo)}IsEmpty";
                return false;
            }

            if (string.IsNullOrEmpty(insurance.PlateNumber))
            {
                message = $"{nameof(InsurancePolicyDto.PlateNumber)}IsEmpty";
                return false;
            }

            insurance.PlateNumber = insurance.PlateNumber.Trim();

            //if (!IsValidIranianNationalCode(customer.NationalCode))
            //{
            //    message = $"{nameof(CustomerDto.NationalCode)}IsInvalid";
            //    return false;
            //}

            if (string.IsNullOrEmpty(insurance.VIN))
            {
                message = $"{nameof(InsurancePolicyDto.VIN)}IsEmpty";
                return false;
            }

            message = null;

            return true;
        }
    }
}

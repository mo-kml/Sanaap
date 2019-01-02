using Sanaap.Dto;

namespace Sanaap.Service.Contracts
{
    public interface IInsuranceValidator
    {
        bool IsValid(InsurancePolicyDto insurance, out string errorMessage);
    }
}

using Sanaap.Dto;

namespace Sanaap.Service.Contracts
{
    public interface ICustomerValidator
    {
        bool IsValid(CustomerDto customer, out string errorMessage);
    }
}
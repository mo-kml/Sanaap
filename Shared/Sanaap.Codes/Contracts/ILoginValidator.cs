using Sanaap.Dto;

namespace Sanaap.Service.Contracts
{
    public interface ILoginValidator
    {
        bool IsValid(string NationalCode, string Mobile, out string errorMessage);
    }
}
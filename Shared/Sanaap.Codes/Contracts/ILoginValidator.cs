using Sanaap.Dto;

namespace Sanaap.Service.Contracts
{
    public interface ILoginValidator
    {
        bool IsValid(LoginDto loginDto, out string errorMessage);
    }
}
using Sanaap.Dto;

namespace Sanaap.Service.Contracts
{
    public interface ILoginValidator
    {
        bool IsValid(string nationalCode, string mobile, out string errorMessage);
    }
}
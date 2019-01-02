using Sanaap.Dto;

namespace Sanaap.Service.Contracts
{
    public interface IEvlRequestValidator
    {
        bool IsValid(EvlRequestDto requestDto, out string errorMessage);
    }
}

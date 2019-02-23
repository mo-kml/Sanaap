using Sanaap.Dto;

namespace Sanaap.Service.Contracts
{
    public interface IEvlRequestValidator
    {
        bool IsDetailValid(EvlRequestDto requestDto, out string errorMessage);
        bool IsLostDetailValid(EvlRequestDto requestDto, out string errorMessage);
    }
}

using Sanaap.Dto;
using Sanaap.Service.Contracts;

namespace Sanaap.Service.Implementations
{
    public class SanaapOperatorAppLoginValidator : ISanaapOperatorAppLoginValidator
    {
        public bool IsValid(string userName, string password, out string message)
        {
            if (string.IsNullOrEmpty(userName))
            {
                message = $"{nameof(OperatorDto.UserName)}IsEmpty";
                return false;
            }

            if (string.IsNullOrEmpty(password))
            {
                message = $"{nameof(OperatorDto.Password)}IsEmpty";
                return false;
            }

            message = null;

            return true;
        }
    }
}

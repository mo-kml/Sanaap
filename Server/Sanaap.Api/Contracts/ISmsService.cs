using System.Threading.Tasks;

namespace Sanaap.Api.Contracts
{
    public interface ISmsService
    {
        Task<string> SendVerifyCode(string mobileNumber);
    }
}

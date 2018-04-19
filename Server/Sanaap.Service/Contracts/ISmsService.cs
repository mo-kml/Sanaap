using System.Threading.Tasks;

namespace Sanaap.Service.Contracts
{
    public interface ISmsService
    {
        Task SendSms(string mobileNo, string message);
    }
}

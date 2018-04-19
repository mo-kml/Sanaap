using Sanaap.Service.Contracts;
using System.Threading.Tasks;

namespace Sanaap.Service.Implementations
{
    public class DefaultSmsService : ISmsService
    {
        public virtual async Task SendSms(string mobileNo, string message)
        {
            
        }
    }
}

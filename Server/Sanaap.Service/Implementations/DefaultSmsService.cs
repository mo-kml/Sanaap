using Sanaap.Service.Contracts;
using System;
using System.Threading.Tasks;

namespace Sanaap.Service.Implementations
{
    public class DefaultSmsService : ISmsService
    {
        public virtual async Task SendSms(string text, string recipient)
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text));

            if (recipient == null)
                throw new ArgumentNullException(nameof(recipient));

            // ...
        }
    }
}

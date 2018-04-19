using Sanaap.Service.Contracts;
using System;

namespace Sanaap.Service.Implementations
{
    public class DefaultOTPNumberGenerator : IOtpNumberGenerator
    {
        private readonly Random _random = new Random();

        public virtual int GetOtpNumber()
        {
            const int min = 1000;
            const int max = 9999;

            return _random.Next(min, max);
        }
    }
}

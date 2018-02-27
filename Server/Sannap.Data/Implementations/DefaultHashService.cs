using Sannap.Data.Contracts;
using System;

namespace Sannap.Data.Implementations
{
    public class DefaultHashService : IHashService
    {
        public virtual string Hash(string input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            return BCrypt.Net.BCrypt.HashPassword(input);
        }

        public virtual bool VerifyHash(string input, string hashedInput)
        {
            return BCrypt.Net.BCrypt.Verify(input, hashedInput);
        }
    }
}

using Sanaap.Service.Contracts;
using System;

namespace Sanaap.Service.Implementations
{
    public class DefaultHashUtils : IHashUtils
    {
        public virtual string HashPassword(string password)
        {
            if (password == null)
            {
                throw new ArgumentNullException(nameof(password));
            }

            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public virtual bool VerifyHash(string input, string hashedInput)
        {
            return BCrypt.Net.BCrypt.Verify(input, hashedInput);
        }
    }
}

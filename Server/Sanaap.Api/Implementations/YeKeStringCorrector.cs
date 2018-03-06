using Bit.Core.Contracts;

namespace Sanaap.Api.Implementations
{
    public class YeKeStringCorrector : IStringCorrector
    {
        public virtual string CorrectString(string input)
        {
            return input?.Replace("ي", "ی").Replace("ك", "ک");
        }
    }
}
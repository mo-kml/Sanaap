namespace Sannap.Data.Contracts
{
    public interface IHashService
    {
        string Hash(string input);

        bool VerifyHash(string input, string hashedInput);
    }
}

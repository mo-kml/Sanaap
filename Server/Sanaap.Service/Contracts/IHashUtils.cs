namespace Sanaap.Data.Contracts
{
    public interface IHashUtils
    {
        string HashPassword(string password);

        bool VerifyHash(string input, string hashedInput);
    }
}

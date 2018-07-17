namespace Sanaap.Service.Contracts
{
    public interface IHashUtils
    {
        string HashPassword(string password);

        bool VerifyHash(string input, string hashedInput);
    }
}

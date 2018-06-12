namespace Sanaap.Service.Contracts
{
    public interface ISanaapOperatorAppLoginValidator
    {
        bool IsValid(string userName, string password, out string errorMessage);
    }
}

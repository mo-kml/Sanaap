namespace Sanaap.Service.Contracts
{
    public interface ISanaapAppLoginValidator
    {
        bool IsValid(string nationalCode, string mobile, out string errorMessage);
    }
}

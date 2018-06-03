namespace Sanaap.Service.Contracts
{
    public interface ISanaapAppTranslateService
    {
        bool Translate(string str, out string translateResult);

        string Translate(string str);
    }
}

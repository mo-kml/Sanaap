namespace Sanaap.Service.Contracts
{
    public interface ITranslateService
    {
        bool Translate(string str, out string translateResult);

        string Translate(string str);
    }
}

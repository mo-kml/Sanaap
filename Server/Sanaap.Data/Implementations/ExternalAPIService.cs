using Sanaap.Data.Contracts;
using System.Net.Http;
using System.Threading.Tasks;

namespace Sanaap.Data.Implementations
{
    public class ExternalAPIService : IExternalAPIService
    {
        private readonly HttpClient _httpClient;
        public ExternalAPIService(HttpClient httpClient)
        {

        }
        private async Task login()
        {

        }
    }
}

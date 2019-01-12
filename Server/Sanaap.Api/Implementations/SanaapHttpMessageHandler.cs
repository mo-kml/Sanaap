using Newtonsoft.Json;
using Sanaap.Api.Contracts;
using Sanaap.Service.Contracts;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sanaap.Api.Implementations
{
    public class SanaapHttpMessageHandler : DelegatingHandler
    {
        public virtual ISanaapTokenService SanaapTokenService { get; set; }

        public virtual IHttpClientFactory HttpClientFactory { get; set; }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Add("auth", SanaapTokenService.Token);

            HttpResponseMessage httpResponse = await base.SendAsync(request, cancellationToken);

            if (httpResponse.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                HttpClient httpClient = HttpClientFactory.CreateClient("SoltaniHttpClient");

                var loginParams = new { Username = "sanap", Password = "10431044" };
                StringContent stringContent = new StringContent(JsonConvert.SerializeObject(loginParams), UnicodeEncoding.UTF8, "application/json");

                HttpRequestMessage loginRequest = new HttpRequestMessage(HttpMethod.Post, httpClient.BaseAddress + "Login");
                loginRequest.Content = stringContent;

                HttpResponseMessage loginResponseMessage = await base.SendAsync(loginRequest, cancellationToken);

                if (loginResponseMessage.IsSuccessStatusCode)
                {
                    SoltaniLoginResponse loginResponse = JsonConvert.DeserializeObject<SoltaniLoginResponse>(await loginResponseMessage.Content.ReadAsStringAsync());

                    SanaapTokenService.Token = loginResponse.token;

                    request.Headers.Remove("auth");

                    request.Headers.Add("auth", SanaapTokenService.Token);

                    httpResponse = await base.SendAsync(request, cancellationToken);
                }
            }

            return httpResponse;
        }
    }
}

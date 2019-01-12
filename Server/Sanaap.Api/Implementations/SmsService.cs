using Newtonsoft.Json;
using Sanaap.Api.Contracts;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Sanaap.Api.Implementations
{
    public class SmsService : ISmsService
    {
        public virtual IHttpClientFactory HttpClientFactory { get; set; }

        public async Task<string> SendVerifyCode(string mobileNumber)
        {
            string code = "";

            Random rand = new Random();

            for (int i = 0; i < 5; i++)
            {
                code += rand.Next(0, 9).ToString();
            }

            HttpClient httpClient = HttpClientFactory.CreateClient("SoltaniHttpClient");
            var loginParams = new { To = mobileNumber, Msg = $"کد ثبت نام در سناپ : {code}" };
            StringContent stringContent = new StringContent(JsonConvert.SerializeObject(loginParams), UnicodeEncoding.UTF8, "application/json");

            await httpClient.PostAsync("SendSms", stringContent);

            return code;
        }
    }
}

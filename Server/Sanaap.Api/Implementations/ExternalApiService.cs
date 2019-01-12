using Newtonsoft.Json;
using Sanaap.Api.Contracts;
using Sanaap.Dto;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Sanaap.Api.Implementations
{
    public class ExternalApiService : IExternalApiService
    {
        public virtual IHttpClientFactory HttpClientFactory { get; set; }

        public virtual ISanaapTokenService SanaapTokenService { get; set; }

        public virtual ISmsService SmsService { get; set; }

        private HttpClient httpClient;
        private IEnumerable<ExternalEntityDto> colors;
        private IEnumerable<ExternalEntityDto> cars;
        private IEnumerable<InsurerDto> insurers;

        public async Task<IEnumerable<ExternalEntityDto>> GetCars()
        {
            if (httpClient == null)
            {
                httpClient = HttpClientFactory.CreateClient("SoltaniHttpClient");
            }

            if (cars == null)
            {
                HttpResponseMessage result = await httpClient.GetAsync("GetCars");

                if (result.IsSuccessStatusCode)
                {
                    cars = JsonConvert.DeserializeObject<IEnumerable<ExternalEntityDto>>(await result.Content.ReadAsStringAsync());
                }
                else
                {
                    throw new Exception(result.ReasonPhrase);
                }
            }

            return cars;
        }

        public async Task<IEnumerable<ExternalEntityDto>> GetColors()
        {
            if (httpClient == null)
            {
                httpClient = HttpClientFactory.CreateClient("SoltaniHttpClient");
            }

            if (colors == null)
            {
                HttpResponseMessage result = await httpClient.GetAsync("GetParameter?type=20");

                if (result.IsSuccessStatusCode)
                {
                    colors = JsonConvert.DeserializeObject<IEnumerable<ExternalEntityDto>>(await result.Content.ReadAsStringAsync());
                }
                else
                {
                    throw new Exception(result.ReasonPhrase);
                }
            }

            return colors;
        }

        public async Task<IEnumerable<InsurerDto>> GetInsurers()
        {
            if (httpClient == null)
            {
                httpClient = HttpClientFactory.CreateClient("SoltaniHttpClient");
            }

            if (insurers == null)
            {
                HttpResponseMessage result = await httpClient.GetAsync("GetInitData");

                if (result.IsSuccessStatusCode)
                {
                    InitialData initialData = JsonConvert.DeserializeObject<InitialData>(await result.Content.ReadAsStringAsync());

                    insurers = initialData.Insurance;
                }
                else
                {
                    throw new Exception(result.ReasonPhrase);
                }

            }

            return insurers;
        }

    }

    public class InitialData
    {
        public List<InsurerDto> Insurance { get; set; }
    }

}

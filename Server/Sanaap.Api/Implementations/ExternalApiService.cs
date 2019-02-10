using Bit.Core.Contracts;
using Newtonsoft.Json;
using Sanaap.Api.Contracts;
using Sanaap.Dto;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Sanaap.Api.Implementations
{
    public class ExternalApiService : IExternalApiService
    {
        public virtual IHttpClientFactory HttpClientFactory { get; set; }

        public virtual IUserInformationProvider UserInformationProvider { get; set; }

        public virtual ISanaapTokenService SanaapTokenService { get; set; }

        public virtual ISmsService SmsService { get; set; }

        private HttpClient httpClient;
        private IEnumerable<ExternalEntityDto> colors;
        private IEnumerable<ExternalEntityDto> cars;
        private IEnumerable<InsurerDto> insurers;
        private IEnumerable<ExternalEntityDto> alphabets;

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

        public async Task<IEnumerable<ExternalEntityDto>> GetNumberplateAlphabets()
        {
            if (httpClient == null)
            {
                httpClient = HttpClientFactory.CreateClient("SoltaniHttpClient");
            }

            if (alphabets == null)
            {
                HttpResponseMessage result = await httpClient.GetAsync("GetParameter?type=31");

                if (result.IsSuccessStatusCode)
                {
                    alphabets = JsonConvert.DeserializeObject<IEnumerable<ExternalEntityDto>>(await result.Content.ReadAsStringAsync());
                }
                else
                {
                    throw new Exception(result.ReasonPhrase);
                }
            }

            return alphabets;
        }

        public async Task<IEnumerable<ContentDto>> GetNews(FilterNewsDto filterNewsDto)
        {
            if (httpClient == null)
            {
                httpClient = HttpClientFactory.CreateClient("SoltaniHttpClient");
            }

            filterNewsDto.Page = 1;

            string json = JsonConvert.SerializeObject(filterNewsDto, Formatting.None,
                        new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        });

            HttpResponseMessage result = await httpClient.PostAsync("GetNewsList", new StringContent(json, UnicodeEncoding.UTF8, "application/json"));

            if (result.IsSuccessStatusCode)
            {
                NewsList newsList = JsonConvert.DeserializeObject<NewsList>(await result.Content.ReadAsStringAsync());

                return newsList.Items;
            }
            else
            {
                throw new Exception(result.ReasonPhrase);
            }

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

        public async Task<ContentDto> GetNewsById(int id)
        {
            if (httpClient == null)
            {
                httpClient = HttpClientFactory.CreateClient("SoltaniHttpClient");
            }

            HttpResponseMessage result = await httpClient.GetAsync($"GetNews?Id={id}&usid={Guid.Parse(UserInformationProvider.GetCurrentUserId())}");

            if (result.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ContentDto>(await result.Content.ReadAsStringAsync());
            }
            else
            {
                throw new Exception(result.ReasonPhrase);
            }
        }


        public async Task<bool> LikeNews(int id)
        {
            if (httpClient == null)
            {
                httpClient = HttpClientFactory.CreateClient("SoltaniHttpClient");
            }

            HttpResponseMessage result = await httpClient.GetAsync($"LikeNews?Id={id}&usid={Guid.Parse(UserInformationProvider.GetCurrentUserId())}");

            if (result.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<bool>(await result.Content.ReadAsStringAsync());
            }
            else
            {
                throw new Exception(result.ReasonPhrase);
            }
        }
    }

    public class InitialData
    {
        public List<InsurerDto> Insurance { get; set; }
    }

}

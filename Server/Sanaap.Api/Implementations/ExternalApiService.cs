using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sanaap.Api.Contracts;
using Sanaap.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static Sanaap.Api.Controllers.EvlRequestExpertsController;

namespace Sanaap.Api.Implementations
{
    //this class is a mess and absolutely needs to optimize
    public class ExternalApiService : IExternalApiService
    {
        public virtual IHttpClientFactory HttpClientFactory { get; set; }

        public virtual ISanaapTokenService SanaapTokenService { get; set; }

        public virtual ISmsService SmsService { get; set; }

        private HttpClient httpClient;
        private IEnumerable<ExternalEntityDto> colors;
        private IEnumerable<ExternalEntityDto> cars;
        private IEnumerable<PhotoTypeDto> salesPhotos;
        private IEnumerable<PhotoTypeDto> badanePhotos;
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

        public async Task<IEnumerable<PhotoTypeDto>> GetSalesPhotos()
        {
            if (httpClient == null)
            {
                httpClient = HttpClientFactory.CreateClient("SoltaniHttpClient");
            }

            if (colors == null)
            {
                HttpResponseMessage result = await httpClient.GetAsync("GetInitData");

                if (result.IsSuccessStatusCode)
                {
                    JObject jObject = JObject.Parse(await result.Content.ReadAsStringAsync());

                    salesPhotos = JsonConvert.DeserializeObject<IEnumerable<PhotoTypeDto>>(jObject["SalesPhotos"].ToString());
                }
                else
                {
                    throw new Exception(result.ReasonPhrase);
                }
            }

            return salesPhotos;
        }

        public async Task<IEnumerable<PhotoTypeDto>> GetBadanePhotos()
        {
            if (httpClient == null)
            {
                httpClient = HttpClientFactory.CreateClient("SoltaniHttpClient");
            }

            if (colors == null)
            {
                HttpResponseMessage result = await httpClient.GetAsync("GetInitData");

                if (result.IsSuccessStatusCode)
                {
                    JObject jObject = JObject.Parse(await result.Content.ReadAsStringAsync());

                    badanePhotos = JsonConvert.DeserializeObject<IEnumerable<PhotoTypeDto>>(jObject["BadanePhotos"].ToString());
                }
                else
                {
                    throw new Exception(result.ReasonPhrase);
                }
            }

            return badanePhotos;
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

                    insurers = initialData.Insurance.Where(i => i.IsContract);
                }
                else
                {
                    throw new Exception(result.ReasonPhrase);
                }

            }

            return insurers;
        }

        public async Task<ContentDto> GetNewsById(int id, Guid userId)
        {
            if (httpClient == null)
            {
                httpClient = HttpClientFactory.CreateClient("SoltaniHttpClient");
            }

            HttpResponseMessage result = await httpClient.GetAsync($"GetNews?Id={id}&usid={userId}");

            if (result.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ContentDto>(await result.Content.ReadAsStringAsync());
            }
            else
            {
                throw new Exception(result.ReasonPhrase);
            }
        }

        public async Task<ExpertPositionDto> GetExpertPosition(GetPositionArgs positionArgs)
        {
            if (httpClient == null)
            {
                httpClient = HttpClientFactory.CreateClient("SoltaniHttpClient");
            }
            StringContent content = new StringContent(JsonConvert.SerializeObject(positionArgs), Encoding.UTF8, "application/json");

            HttpResponseMessage result = await httpClient.PostAsync($"GetExpertPosition", content);


            if (result.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ExpertPositionDto>(await result.Content.ReadAsStringAsync());
            }
            else
            {
                throw new Exception(result.ReasonPhrase);
            }
        }


        public async Task<bool> LikeNews(int id, Guid userId)
        {
            if (httpClient == null)
            {
                httpClient = HttpClientFactory.CreateClient("SoltaniHttpClient");
            }

            HttpResponseMessage result = await httpClient.GetAsync($"LikeNews?Id={id}&usid={userId}");

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

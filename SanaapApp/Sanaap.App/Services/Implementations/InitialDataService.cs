using Newtonsoft.Json;
using Sanaap.App.Services.Contracts;
using Sanaap.Dto;
using Simple.OData.Client;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Sanaap.App.Services.Implementations
{
    public class InitialDataService : IInitialDataService
    {
        private readonly HttpClient _httpClient;
        private readonly IODataClient _oDataClient;
        private IEnumerable<ExternalEntityDto> cars;
        private IEnumerable<ExternalEntityDto> colors;
        private CustomerDto customerDto;

        public InitialDataService(HttpClient httpClient, IODataClient oDataClient)
        {
            _httpClient = httpClient;
            _oDataClient = oDataClient;
        }
        public async Task<IEnumerable<ExternalEntityDto>> GetCars()
        {
            if (cars == null)
            {
                string json = JsonConvert.SerializeObject(new { SearchKey = "", Type = 414 });

                StringContent stringContent = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage result = await _httpClient.GetAsync("GetCars");

                if (result.IsSuccessStatusCode)
                {
                    cars = JsonConvert.DeserializeObject<IEnumerable<ExternalEntityDto>>(await result.Content.ReadAsStringAsync());
                }
                else
                {
                    throw new System.Exception(result.ReasonPhrase);
                }
            }

            return cars;
        }

        public async Task<IEnumerable<ExternalEntityDto>> GetColors()
        {
            if (colors == null)
            {
                HttpResponseMessage result = await _httpClient.GetAsync("GetParameter?type=20");

                if (result.IsSuccessStatusCode)
                {
                    colors = JsonConvert.DeserializeObject<IEnumerable<ExternalEntityDto>>(await result.Content.ReadAsStringAsync());
                }
                else
                {
                    throw new System.Exception(result.ReasonPhrase);
                }
            }

            return colors;
        }

        public async Task<CustomerDto> GetCurrentUserInfo()
        {
            if (customerDto == null)
            {
                customerDto = await _oDataClient.For<CustomerDto>("Customers")
                    .Function("GetCurrentCustomer")
                    .FindEntryAsync();
            }

            return customerDto;
        }
    }
}

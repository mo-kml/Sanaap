using Newtonsoft.Json;
using Sanaap.App.ItemSources;
using Sanaap.App.Services.Contracts;
using Sanaap.Dto;
using Simple.OData.Client;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Sanaap.App.Services.Implementations
{
    public class PolicyService : DefaultService<InsurancePolicyDto>, IPolicyService
    {
        private readonly IODataClient _oDataClient;
        private readonly HttpClient _httpClient;
        public PolicyService(HttpClient httpClient, IODataClient oDataClient) : base(oDataClient)
        {
            _oDataClient = oDataClient;
            _httpClient = httpClient;
        }

        public async Task<List<PolicyItemSource>> LoadAllInsurances()
        {
            List<ExternalEntityDto> cars = new List<ExternalEntityDto>();
            List<ExternalEntityDto> colors = new List<ExternalEntityDto>();
            List<PolicyItemSource> policyItemSource = new List<PolicyItemSource>();

            IEnumerable<InsurancePolicyDto> insurances = await _oDataClient.For<InsurancePolicyDto>(controllerName)
                .FindEntriesAsync();


            HttpResponseMessage carsResult = await _httpClient.GetAsync("Cars/GetAll");

            if (carsResult.IsSuccessStatusCode)
            {
                cars = JsonConvert.DeserializeObject<List<ExternalEntityDto>>(await carsResult.Content.ReadAsStringAsync());
            }
            else
            {
                throw new System.Exception();
            }

            HttpResponseMessage colorsResult = await _httpClient.GetAsync("Colors/GetAll");

            if (colorsResult.IsSuccessStatusCode)
            {
                colors = JsonConvert.DeserializeObject<List<ExternalEntityDto>>(await colorsResult.Content.ReadAsStringAsync());
            }
            else
            {
                throw new System.Exception();
            }

            foreach (InsurancePolicyDto insurance in insurances)
            {
                policyItemSource.Add(new PolicyItemSource
                {
                    CarId = insurance.CarId,
                    InsuranceType = insurance.InsuranceType,
                    ChasisNo = insurance.ChasisNo,
                    ColorId = insurance.ColorId,
                    Id = insurance.Id,
                    InsurerNo = insurance.InsurerNo,
                    PlateNumber = insurance.PlateNumber,
                    VIN = insurance.VIN,
                    ColorName = colors.FirstOrDefault(c => c.Id == insurance.ColorId)?.Name,
                    CarName = cars.FirstOrDefault(c => c.Id == insurance.CarId)?.Name,
                });
            }

            return policyItemSource;
        }
    }
}

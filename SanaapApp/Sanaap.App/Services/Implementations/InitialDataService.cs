using Sanaap.App.ItemSources;
using Sanaap.App.Services.Contracts;
using Sanaap.Dto;
using Simple.OData.Client;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Sanaap.App.Services.Implementations
{
    public class InitialDataService : IInitialDataService
    {
        private readonly HttpClient _httpClient;
        private readonly IODataClient _oDataClient;
        private List<ExternalEntityDto> cars;
        private List<ExternalEntityDto> alphabets;
        private List<ExternalEntityDto> colors;
        private List<InsurersItemSource> insurers;
        private CustomerDto customerDto;

        public InitialDataService(HttpClient httpClient, IODataClient oDataClient)
        {
            _httpClient = httpClient;
            _oDataClient = oDataClient;
        }
        public async Task<List<ExternalEntityDto>> GetCars()
        {
            if (cars == null)
            {
                cars = (await _oDataClient.For<ExternalEntityDto>("ExternalEntities")
                    .Function("GetCars")
                    .FindEntriesAsync()).ToList();
            }

            return cars;
        }

        public async Task<List<ExternalEntityDto>> GetColors()
        {
            if (colors == null)
            {
                colors = (await _oDataClient.For<ExternalEntityDto>("ExternalEntities")
                    .Function("GetColors")
                    .FindEntriesAsync()).ToList();
            }

            return colors;
        }

        public async Task<List<ExternalEntityDto>> GetAlphabets()
        {
            if (alphabets == null)
            {
                alphabets = (await _oDataClient.For<ExternalEntityDto>("ExternalEntities")
                    .Function("GetAlphabets")
                    .FindEntriesAsync()).ToList();
            }

            return alphabets;
        }

        public async Task<List<InsurersItemSource>> GetInsurers()
        {
            if (insurers == null)
            {
                List<InsurerDto> insurerDtos = (await _oDataClient.For<InsurerDto>("Insurers")
                    .Function("GetInsurers")
                    .FindEntriesAsync()).ToList();

                insurers = new List<InsurersItemSource>();

                foreach (InsurerDto i in insurerDtos)
                {
                    insurers.Add(new InsurersItemSource
                    {
                        ID = i.ID,
                        Image = ImageSource.FromUri(new System.Uri(i.Photo)),
                        Photo = i.Photo,
                        IsContract = i.IsContract,
                        Name = i.Name
                    });
                }
            }

            return insurers;
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

        public async Task<MonthItemSource[]> GetMonths()
        {
            return new MonthItemSource[]
            {
                new MonthItemSource{Name="فروردین",Number=1},
                new MonthItemSource{Name="اردیبهشت",Number=2},
                new MonthItemSource{Name="خرداد",Number=3},
                new MonthItemSource{Name="تیر",Number=4},
                new MonthItemSource{Name="مرداد",Number=5},
                new MonthItemSource{Name="شهریور",Number=6},
                new MonthItemSource{Name="مهر",Number=7},
                new MonthItemSource{Name="آبان",Number=8},
                new MonthItemSource{Name="آذر",Number=9},
                new MonthItemSource{Name="دی",Number=10},
                new MonthItemSource{Name="بهمن",Number=11},
                new MonthItemSource{Name="اسفند",Number=12},
            };
        }

        public async Task<YearItemSource[]> GetYears()
        {
            return new YearItemSource[] {
               new YearItemSource{Number=1397},
               new YearItemSource{Number=1398},
               new YearItemSource{Number=1399},
               new YearItemSource{Number=1400},
               new YearItemSource{Number=1401},
               new YearItemSource{Number=1402},
               new YearItemSource{Number=1403},
            };
        }
    }
}

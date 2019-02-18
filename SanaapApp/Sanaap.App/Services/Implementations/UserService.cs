using Sanaap.App.Services.Contracts;
using Sanaap.Dto;
using Simple.OData.Client;
using System.Threading.Tasks;

namespace Sanaap.App.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IODataClient _oDataClient;
        public UserService(IODataClient oDataClient)
        {
            _oDataClient = oDataClient;
        }
        public async Task<CustomerDto> ActiveUser()
        {
            return await _oDataClient.For<CustomerDto>("Customers")
                .Action("ActiveCustomer")
                .ExecuteAsSingleAsync();
        }
    }
}

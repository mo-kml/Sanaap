using Sanaap.App.Services.Contracts;
using Sanaap.Dto;
using Simple.OData.Client;

namespace Sanaap.App.Services.Implementations
{
    public class EvlRequestService : DefaultService<EvlRequestDto>, IEvlRequestService
    {
        public EvlRequestService(IODataClient oDataClient) : base(oDataClient)
        {
        }
    }
}

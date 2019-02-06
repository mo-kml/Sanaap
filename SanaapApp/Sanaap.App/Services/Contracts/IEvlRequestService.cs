using Sanaap.App.ItemSources;
using Sanaap.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sanaap.App.Services.Contracts
{
    public interface IEvlRequestService : IService<EvlRequestDto>
    {
        Task<IEnumerable<EvlRequestListItemSource>> GetAllRequests();

        Task<IEnumerable<ProgressItemSource>> GetAllProgressesByRequestId(Guid requestId);

        Task<EvlRequestDto> SearchByCode(int code);

        Task<EvlRequestExpertDto> FindEvlRequestExpert(Guid evlRequestId);
    }
}

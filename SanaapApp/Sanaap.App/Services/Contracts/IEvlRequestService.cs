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

        Task<EvlRequestDto> SearchByCode(int code);

        Task<EvlRequestExpertDto> FindEvlRequestExpert(Guid evlRequestId);

        Task<ExpertPositionDto> UpdateExpertPosition(string token);

        Task<IEnumerable<ProgressItemSource>> GetAllProgressesByRequestId(int fileId);

        Task<EvlRequestDto> UpdateRank(EvlRequestDto evlRequest);
    }
}

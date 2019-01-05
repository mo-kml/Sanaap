using Sanaap.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sanaap.App.Services.Contracts
{
    public interface IEvlRequestService : IService<EvlRequestDto>
    {
        Task<IEnumerable<EvlRequestDto>> GetAllRequests();

        Task<IEnumerable<EvlRequestProgressDto>> GetAllProgressesByRequestId(Guid requestId);
    }
}

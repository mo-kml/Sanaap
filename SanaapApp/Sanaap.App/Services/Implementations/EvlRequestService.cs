﻿using Sanaap.App.Services.Contracts;
using Sanaap.Dto;
using Simple.OData.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sanaap.App.Services.Implementations
{
    public class EvlRequestService : DefaultService<EvlRequestDto>, IEvlRequestService
    {
        private readonly IODataClient _oDataClient;
        public EvlRequestService(IODataClient oDataClient) : base(oDataClient)
        {
            _oDataClient = oDataClient;
        }

        public async Task<EvlRequestExpertDto> FindEvlRequestExpert(Guid evlRequestId)
        {
            return await _oDataClient.For<EvlRequestExpertDto>("EvlRequestExperts")
                .Set(evlRequestId)
                .Function("FindEvlRequestExpert")
                .FindEntryAsync();
        }

        public async Task<IEnumerable<EvlRequestProgressDto>> GetAllProgressesByRequestId(Guid requestId)
        {
            return await _oDataClient.For<EvlRequestProgressDto>("EvlRequestProgresses")
                .Filter(x => x.EvlRequestId == requestId)
                .FindEntriesAsync();
        }

        public async Task<IEnumerable<EvlRequestDto>> GetAllRequests()
        {
            return await _oDataClient.For<EvlRequestDto>(controllerName)
                .Function("GetCustomerEvlRequests")
                .FindEntriesAsync();
        }

        public async Task<EvlRequestDto> SearchByCode(int code)
        {
            return await _oDataClient.For<EvlRequestDto>(controllerName)
               .Filter(x => x.Code == code)
               .FindEntryAsync();
        }
    }
}

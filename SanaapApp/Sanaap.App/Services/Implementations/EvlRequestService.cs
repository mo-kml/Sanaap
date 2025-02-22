﻿using Newtonsoft.Json;
using Sanaap.App.Helpers.Contracts;
using Sanaap.App.ItemSources;
using Sanaap.App.Services.Contracts;
using Sanaap.Dto;
using Sanaap.Enums;
using Simple.OData.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sanaap.App.Services.Implementations
{
    public class EvlRequestService : DefaultService<EvlRequestDto>, IEvlRequestService
    {
        private readonly IODataClient _oDataClient;
        private readonly IDateHelper _dateHelper;
        public EvlRequestService(IODataClient oDataClient, IDateHelper dateHelper) : base(oDataClient)
        {
            _oDataClient = oDataClient;
            _dateHelper = dateHelper;
        }

        public async Task<EvlRequestExpertDto> FindEvlRequestExpert(Guid evlRequestId)
        {
            return await _oDataClient.For<EvlRequestExpertDto>("EvlRequestExperts")
                .Action("FindEvlRequestExpert")
                .Set(new { requestId = evlRequestId })
                .ExecuteAsSingleAsync();
        }

        public async Task<EvlRequestDto> UpdateRank(EvlRequestDto evlRequest)
        {
            return await _oDataClient.For<EvlRequestDto>(controllerName)
                .Action("UpdateRank")
                .Set(new { evlRequestId = evlRequest.Id, Rank = evlRequest.RankValue, Description = evlRequest.RankDescription })
                .ExecuteAsSingleAsync();
        }

        public async Task<IEnumerable<ProgressItemSource>> GetAllProgressesByRequestId(int fileId)
        {
            IEnumerable<EvlRequestProgressDto> progresses = await _oDataClient.For<EvlRequestProgressDto>("EvlRequestProgresses")
                .Set(new { fileId })
                .Function("GetAllByRequestId")
                .FindEntriesAsync();

            List<ProgressItemSource> itemSource = new List<ProgressItemSource>();
            foreach (EvlRequestProgressDto progress in progresses)
            {
                itemSource.Add(new ProgressItemSource
                {
                    Date = _dateHelper.ToPersianShortDate(progress.STime.Date),
                    Time = progress.STime.ToLocalTime().ToString("HH:mm"),
                    Status = progress.Desc
                });
            }

            itemSource[itemSource.Count - 1].IsLatest = true;

            return itemSource;
        }

        public async Task<IEnumerable<EvlRequestListItemSource>> GetAllRequests()
        {
            IEnumerable<EvlRequestDto> requests = await _oDataClient.For<EvlRequestDto>(controllerName)
                .Function("GetCustomerEvlRequests")
                .FindEntriesAsync();

            List<EvlRequestListItemSource> itemSource = new List<EvlRequestListItemSource>();

            foreach (EvlRequestDto request in requests)
            {
                itemSource.Add(new EvlRequestListItemSource
                {
                    Code = request.Code,
                    RequestTypeName = EnumHelper<EvlRequestType>.GetDisplayValue(request.EvlRequestType),
                    Date = _dateHelper.ToPersianShortDate(request.CreatedOn.Date),
                    RequestId = request.Id
                });
            }

            return itemSource;

        }

        public async Task<EvlRequestDto> SearchByCode(int code)
        {
            return await _oDataClient.For<EvlRequestDto>(controllerName)
               .Filter(x => x.Code == code)
               .FindEntryAsync();
        }

        public async Task<ExpertPositionDto> UpdateExpertPosition(string token)
        {
            string expertPosition = await _oDataClient.For<EvlRequestExpertDto>("EvlRequestExperts")
               .Action("GetExpertPosition")
               .Set(new { Token = token })
               .ExecuteAsScalarAsync<string>();

            return JsonConvert.DeserializeObject<ExpertPositionDto>(expertPosition);
        }
    }
}

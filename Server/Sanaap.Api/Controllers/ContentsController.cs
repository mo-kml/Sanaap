﻿using Bit.Core.Contracts;
using Bit.OData.ODataControllers;
using Sanaap.Api.Contracts;
using Sanaap.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sanaap.Api.Controllers
{
    public class ContentsController : DtoController<ContentDto>
    {
        public virtual IExternalApiService ExternalApiService { get; set; }

        public virtual IUserInformationProvider UserInformationProvider { get; set; }


        [Function]
        public async Task<IEnumerable<ContentDto>> GetNews()
        {
            return await ExternalApiService.GetNews();
        }

        [Function]
        public async Task<ContentDto> GetNewsById(int newsId)
        {
            return await ExternalApiService.GetNewsById(newsId, Guid.Parse(UserInformationProvider.GetCurrentUserId()));
        }
    }
}

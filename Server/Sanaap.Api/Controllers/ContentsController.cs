using Bit.OData.ODataControllers;
using Sanaap.Api.Contracts;
using Sanaap.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace Sanaap.Api.Controllers
{
    public class ContentsController : DtoController<ContentDto>
    {
        public virtual IExternalApiService ExternalApiService { get; set; }


        [Action]
        public async Task<IEnumerable<ContentDto>> GetNews([FromBody]FilterNewsDto filterNewsDto)
        {
            return await ExternalApiService.GetNews(filterNewsDto);
        }

        [Function]
        public async Task<ContentDto> GetNewsById(int newsId)
        {
            return await ExternalApiService.GetNewsById(newsId);
        }

        [Function]
        public async Task<bool> LikeNews(int newsId)
        {
            return await ExternalApiService.LikeNews(newsId);
        }
    }
}

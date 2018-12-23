using Bit.Model.Contracts;
using Bit.OData.ODataControllers;
using Sanaap.Data.Contracts;
using Sanaap.Dto;
using Sanaap.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sanaap.Api.Controllers
{
    public class ContentListsController : DtoController<ContentListDto>
    {
        public virtual ISanaapRepository<Content> ContentsRepository { get; set; }
        public virtual ISanaapRepository<ContentLike> ContentLikesRepository { get; set; }

        public virtual IDtoEntityMapper<ContentDto, Content> DtoEntityMapper { get; set; }

        [Function]
        public async Task<List<ContentListDto>> GetAllContents(CancellationToken cancellationToken)
        {
            List<ContentDto> contents = new List<ContentDto>
            {
                new ContentDto
                {
                    CategoryId=Guid.NewGuid(),
                    CreatedOn=DateTimeOffset.Now,
                    ModifiedOn=DateTimeOffset.Now,
                    Text="fdsfad",
                    Title="fadfsa",
                    UserId=Guid.NewGuid(),
                    Id=Guid.NewGuid()
                },
                new ContentDto
                {
                    CategoryId=Guid.NewGuid(),
                    CreatedOn=DateTimeOffset.Now,
                    ModifiedOn=DateTimeOffset.Now,
                    Text="fdsfad",
                    Title="fadfsa",
                    UserId=Guid.NewGuid(),
                    Id=Guid.NewGuid()
                },new ContentDto
                {
                    CategoryId=Guid.NewGuid(),
                    CreatedOn=DateTimeOffset.Now,
                    ModifiedOn=DateTimeOffset.Now,
                    Text="fdsfad",
                    Title="fadfsa",
                    UserId=Guid.NewGuid(),
                    Id=Guid.NewGuid()
                },new ContentDto
                {
                    CategoryId=Guid.NewGuid(),
                    CreatedOn=DateTimeOffset.Now,
                    ModifiedOn=DateTimeOffset.Now,
                    Text="fdsfad",
                    Title="fadfsa",
                    UserId=Guid.NewGuid(),
                    Id=Guid.NewGuid()
                },
            };
            //return DtoEntityMapper.FromEntityQueryToDtoQuery(await ContentsRepository.GetAllAsync(cancellationToken));

            List<ContentListDto> contentList = new List<ContentListDto>();

            foreach (ContentDto content in contents)
            {
                contentList.Add(new ContentListDto
                {
                    CategoryId = content.CategoryId,
                    CreateDate = content.CreatedOn,
                    Id = content.Id,
                    Image = content.Image,
                    Title = content.Title,
                    LikesCount = (await ContentLikesRepository.GetAllAsync(cancellationToken)).Count(l => l.ContentId == content.Id)
                });
            }

            return contentList;
        }
    }
}

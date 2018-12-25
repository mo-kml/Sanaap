using Bit.Core.Contracts;
using Bit.Data.Contracts;
using Bit.Model.Contracts;
using Bit.OData.ODataControllers;
using Sanaap.Dto;
using Sanaap.Model;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sanaap.Api.Controllers
{
    public class CommentsController : SanaapDtoSetController<CommentDto, Comment>
    {
        public virtual IUserInformationProvider UserInformationProvider { get; set; }

        public virtual IDtoEntityMapper<CommentDto, Comment> Mapper { get; set; }

        public virtual IRepository<Comment> Repository { get; set; }

        [Function]
        public virtual async Task<IQueryable<CommentDto>> LoadComments(CancellationToken cancellationToken)
        {
            Guid customerId = Guid.Parse(UserInformationProvider.GetCurrentUserId());

            return Mapper.FromEntityQueryToDtoQuery((await Repository
                .GetAllAsync(cancellationToken))
                .Where(comment => comment.CustomerId == customerId));
        }

        public override Task<CommentDto> Create(CommentDto dto, CancellationToken cancellationToken)
        {
            Guid customerId = Guid.Parse(UserInformationProvider.GetCurrentUserId());

            dto.CustomerId = customerId;

            dto.Code = DateTimeOffset.UtcNow.Ticks;

            return base.Create(dto, cancellationToken);
        }
    }
}

using Sanaap.App.ViewModels.Comment;
using Sanaap.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sanaap.App.Services.Contracts
{
    public interface ICommentService : IService<CommentDto>
    {
        Task<IEnumerable<CommentItemSource>> LoadComments();
    }
}

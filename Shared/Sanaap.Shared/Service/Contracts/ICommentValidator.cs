using Sanaap.Dto;

namespace Sanaap.Service.Contracts
{
    public interface ICommentValidator
    {
        bool IsValid(CommentDto comment, out string errorMessage);
    }
}

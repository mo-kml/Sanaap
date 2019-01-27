using Sanaap.App.Services.Contracts;
using Sanaap.App.ViewModels.Comment;
using Sanaap.Dto;
using Simple.OData.Client;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sanaap.App.Services.Implementations
{
    public class CommentService : DefaultService<CommentDto>, ICommentService
    {
        private IODataClient _oDataClient;
        public CommentService(IODataClient oDataClient) : base(oDataClient)
        {
            _oDataClient = oDataClient;
        }

        public async Task<IEnumerable<CommentItemSource>> LoadComments()
        {
            IEnumerable<CommentDto> comments = (await _oDataClient.For<CommentDto>(controllerName)
                .Function("LoadComments")
                .OrderBy(it => it.CreatedOn)
                .FindEntriesAsync());

            return comments.Select(c => new CommentItemSource
            {
                Answer = c.Answer,
                AnswerTime = c.AnswerTime,
                CommentType = c.CommentType,
                CreatedOn = c.CreatedOn,
                Description = c.Description,
                FirstName = c.FirstName,
                LastName = c.LastName,
                Id = c.Id,
                Mobile = c.Mobile,
                StatusType = c.StatusType,
                Code = c.Code
            });
        }
    }
}

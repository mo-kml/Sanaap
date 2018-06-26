using Bit.Model.Contracts;
using Sanaap.App.Dto;
using Sanaap.Model;
using Sannap.Data.Contracts;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace Sanaap.Api.Controllers
{
    [RoutePrefix("RequestFiles")]
    public class RequestFilesController : ApiController
    {
        public virtual ISanaapRepository<RequestFiles> RequestFileRepository { get; set; }

        public virtual IDtoEntityMapper<RequestFilesDto, RequestFiles> DtoEntityMapper { get; set; }

        [HttpPost, Route("SaveFile")]
        public virtual async Task<RequestFilesDto> SaveFile(RequestFilesDto requestFile, CancellationToken cancellationToken)
        {
            return DtoEntityMapper.FromEntityToDto(await RequestFileRepository.AddAsync(DtoEntityMapper.FromDtoToEntity(requestFile), cancellationToken));
        }
    }
}

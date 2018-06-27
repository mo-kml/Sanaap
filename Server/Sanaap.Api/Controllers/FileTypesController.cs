using Bit.Model.Contracts;
using Sanaap.App.Dto;
using Sanaap.Model;
using Sannap.Data.Contracts;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace Sanaap.Api.Controllers
{
    [RoutePrefix("FileTypes")]
    public class FileTypesController : ApiController
    {
        public virtual ISanaapRepository<FileType> FileTypeRepository { get; set; }

        public virtual IDtoEntityMapper<FileTypeDto, FileType> DtoEntityMapper { get; set; }

        [HttpGet, Route("GetAll")]
        public virtual async Task<IQueryable<FileTypeDto>> GetAll(CancellationToken cancellationToken)
        {
            return DtoEntityMapper.FromEntityQueryToDtoQuery(await FileTypeRepository.GetAllAsync(cancellationToken));
        }
    }
}

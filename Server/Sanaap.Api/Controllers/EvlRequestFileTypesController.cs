using Bit.Model.Contracts;
using Bit.OData.ODataControllers;
using Sanaap.App.Dto;
using Sanaap.Model;
using Sanaap.Data.Contracts;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sanaap.Api.Controllers
{
    public class EvlRequestFileTypesController : DtoController<EvlRequestFileTypeDto>
    {
        public virtual ISanaapRepository<EvlRequestFileType> EvlRequestFileTypesRepository { get; set; }

        public virtual IDtoEntityMapper<EvlRequestFileTypeDto, EvlRequestFileType> DtoEntityMapper { get; set; }

        [Get]
        public virtual async Task<IQueryable<EvlRequestFileTypeDto>> GetAll(CancellationToken cancellationToken)
        {
            return DtoEntityMapper.FromEntityQueryToDtoQuery(await EvlRequestFileTypesRepository.GetAllAsync(cancellationToken));
        }
    }
}

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
    public class CompaniesController : DtoController<CompanyDto>
    {
        public virtual ISanaapRepository<Company> CompaniesRepository { get; set; }

        public virtual IDtoEntityMapper<CompanyDto, Company> DtoEntityMapper { get; set; }

        [Get]
        public virtual async Task<IQueryable<CompanyDto>> GetAll(CancellationToken cancellationToken)
        {
            return DtoEntityMapper.FromEntityQueryToDtoQuery(await CompaniesRepository.GetAllAsync(cancellationToken));
        }
    }
}

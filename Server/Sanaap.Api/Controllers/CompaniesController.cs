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
    [RoutePrefix("Companies")]
    public class CompaniesController : ApiController
    {
        public virtual ISanaapRepository<Company> CompanyRepository { get; set; }

        public virtual IDtoEntityMapper<CompanyDto, Company> DtoEntityMapper { get; set; }

        [HttpGet, Route("GetAll")]
        public virtual async Task<IQueryable<CompanyDto>> GetAll(CancellationToken cancellationToken)
        {
            return DtoEntityMapper.FromEntityQueryToDtoQuery(await CompanyRepository.GetAllAsync(cancellationToken));
        }
    }
}

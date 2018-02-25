using Sanaap.Dto;
using Sanaap.Model;
using System.Web.Http;

namespace Sanaap.Api.Controllers
{
    //[AllowAnonymous]
    public class CitiesController : SanaapDtoSetController<CityDto, City>
    {
    }
}

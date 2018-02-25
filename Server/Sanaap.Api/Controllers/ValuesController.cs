using System.Web.Http;

namespace Sanaap.Api.Controllers
{
    public class ValuesController : ApiController
    {
        [HttpGet]
        //[AllowAnonymous]
        public int Sum(int a, int b)
        {
            return a + b;
        }
    }
}
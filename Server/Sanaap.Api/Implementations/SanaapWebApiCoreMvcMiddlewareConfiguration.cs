using Bit.OwinCore.Contracts;
using Bit.OwinCore.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;

namespace Sanaap.Api.Implementations
{
    [Controller]
    public class PeopleController // api-core/People/GetData/
    {
        [HttpGet]
        public virtual JsonResult GetData()
        {
            return new JsonResult(new { FirstName = "Test" });
        }
    }

    public class SanaapWebApiCoreMvcMiddlewareConfiguration : IAspNetCoreMiddlewareConfiguration
    {
        public virtual void Configure(IApplicationBuilder aspNetCoreApp)
        {
            aspNetCoreApp.Map("/api-core", innerAspNetCoreApp =>
            {
                innerAspNetCoreApp.UseMvcWithDefaultRoute();

                innerAspNetCoreApp.UseMiddleware<AspNetCoreNoCacheResponseMiddleware>();
            });
        }
    }
}

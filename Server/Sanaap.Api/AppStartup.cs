using Bit.Core;
using Bit.Core.Contracts;
using Bit.Core.Implementations;
using Bit.Core.Models;
using Bit.Data;
using Bit.Data.Contracts;
using Bit.Model.Implementations;
using Bit.OData.ActionFilters;
using Bit.OData.Contracts;
using Bit.Owin.Implementations;
using Bit.OwinCore;
using Microsoft.Extensions.DependencyInjection;
using Sanaap.Api.Implementations;
using Sanaap.Api.Implementations.Security;
using Sanaap.Data;
using Sanaap.Data.Implementations;
using Sanaap.Service.Contracts;
using Sanaap.Service.Implementations;
using Swashbuckle.Application;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection;

[assembly: ODataModule("Sanaap")]

namespace Sanaap.Api
{
    public class AppStartup : AutofacAspNetCoreAppStartup
    {
        public AppStartup(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {

        }

        public override IServiceProvider ConfigureServices(IServiceCollection services)
        {
            DefaultAppModulesProvider.Current = new SanaapAppModulesProvider();

            return base.ConfigureServices(services);
        }
    }

    public class SanaapAppModulesProvider : IAppModulesProvider, IAppModule
    {
        public virtual void ConfigureDependencies(IServiceCollection services, IDependencyManager dependencyManager)
        {
            AssemblyContainer.Current.Init();

            dependencyManager.RegisterMinimalDependencies();

            dependencyManager.RegisterDefaultLogger(typeof(DebugLogStore).GetTypeInfo(), typeof(ConsoleLogStore).GetTypeInfo(), typeof(WindowsEventsLogStore).GetTypeInfo());

            dependencyManager.Register<IDbConnectionProvider, DefaultDbConnectionProvider<SqlConnection>>();

            dependencyManager.RegisterDefaultAspNetCoreApp();

            dependencyManager.RegisterMinimalAspNetCoreMiddlewares();

            dependencyManager.RegisterAspNetCoreSingleSignOnClient();

            dependencyManager.RegisterDefaultWebApiAndODataConfiguration();

            dependencyManager.RegisterWebApiMiddleware(webApiDependencyManager =>
            {
                webApiDependencyManager.RegisterGlobalWebApiActionFiltersUsing(httpConfiguration =>
                {
                    httpConfiguration.Filters.Add(new System.Web.Http.AuthorizeAttribute());
                });

                webApiDependencyManager.RegisterGlobalWebApiCustomizerUsing(httpConfiguration =>
                {
                    httpConfiguration.EnableSwagger(c =>
                    {
                        EnvironmentAppInfo appInfo = DefaultAppEnvironmentsProvider.Current.GetActiveAppEnvironment().AppInfo;
                        c.SingleApiVersion($"v{appInfo.Version}", $"{appInfo.Name}-Api");
                        c.ApplyDefaultApiConfig(httpConfiguration);
                    }).EnableBitSwaggerUi();
                });

                webApiDependencyManager.RegisterWebApiMiddlewareUsingDefaultConfiguration();
            });

            dependencyManager.RegisterODataMiddleware(odataDependencyManager =>
            {
                odataDependencyManager.RegisterGlobalWebApiActionFiltersUsing(httpConfiguration =>
                {
                    httpConfiguration.Filters.Add(new DefaultODataAuthorizeAttribute());
                });

                odataDependencyManager.RegisterGlobalWebApiCustomizerUsing(httpConfiguration =>
                {
                    httpConfiguration.EnableSwagger(c =>
                    {
                        EnvironmentAppInfo appInfo = DefaultAppEnvironmentsProvider.Current.GetActiveAppEnvironment().AppInfo;
                        c.SingleApiVersion($"v{appInfo.Version}", $"{appInfo.Name}-Api");
                        c.ApplyDefaultODataConfig(httpConfiguration);
                    }).EnableBitSwaggerUi();
                });

                odataDependencyManager.RegisterWebApiODataMiddlewareUsingDefaultConfiguration();
            });

            dependencyManager.RegisterRepository(typeof(SanaapEfRepository<>).GetTypeInfo());

            dependencyManager.RegisterEfDbContext<SanaapDbContext>();

            dependencyManager.RegisterDtoEntityMapper();

            dependencyManager.RegisterDtoEntityMapperConfiguration<DefaultDtoEntityMapperConfiguration>();
            dependencyManager.RegisterDtoEntityMapperConfiguration<SanaapDtoEntityMapperConfiguration>();

            dependencyManager.RegisterSingleSignOnServer<SanaapUserService, SanaapOAuthClientsProvider>();

            dependencyManager.Register<IStringCorrector, YeKeStringCorrector>(overwriteExciting: false);

            dependencyManager.Register<ICustomerValidator, DefaultCustomerValidator>();
            dependencyManager.Register<ISanaapAppTranslateService, SanaapAppTranslateService>(lifeCycle: DependencyLifeCycle.SingleInstance);

            dependencyManager.Register<IHashUtils, DefaultHashUtils>(lifeCycle: DependencyLifeCycle.SingleInstance);

            services.AddHttpClient("SoltaniHttpClient", httpClient =>
            {
                httpClient.BaseAddress = new Uri("http://5.144.128.234:8800/");
            });
        }

        public IEnumerable<IAppModule> GetAppModules()
        {
            yield return this;
        }
    }
}

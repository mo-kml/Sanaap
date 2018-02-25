﻿using Bit.Core;
using Bit.Core.Contracts;
using Bit.Core.Implementations;
using Bit.Core.Models;
using Bit.Data;
using Bit.Data.Contracts;
using Bit.Model.Implementations;
using Bit.OData.ActionFilters;
using Bit.OData.Implementations;
using Bit.Owin.Implementations;
using Bit.OwinCore;
using Bit.OwinCore.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Sanaap.Api.Implementations;
using Sanaap.Api.Implementations.Security;
using Sanaap.Dto.Implementations;
using Sanaap.Service.Contracts;
using Sanaap.Service.Implementations;
using Sannap.Data;
using Sannap.Data.Implementations;
using Swashbuckle.Application;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection;

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
            DefaultDependenciesManagerProvider.Current = new SanaapDependenciesManager();

            return base.ConfigureServices(services);
        }
    }

    public class SanaapDependenciesManager : IDependenciesManagerProvider, IAspNetCoreDependenciesManager
    {
        public virtual void ConfigureDependencies(IServiceProvider serviceProvider, IServiceCollection services, IDependencyManager dependencyManager)
        {
            AssemblyContainer.Current.Init();

            dependencyManager.RegisterMinimalDependencies();

            dependencyManager.RegisterDefaultLogger(typeof(DebugLogStore).GetTypeInfo(), typeof(ConsoleLogStore).GetTypeInfo());

            dependencyManager.Register<IDbConnectionProvider, DefaultDbConnectionProvider<SqlConnection>>();

            dependencyManager.RegisterDefaultAspNetCoreApp();

            dependencyManager.RegisterMinimalAspNetCoreMiddlewares();

            dependencyManager.RegisterAspNetCoreSingleSignOnClient();

            services.AddWebApiCore();
            dependencyManager.RegisterAspNetCoreMiddleware<SanaapWebApiCoreMvcMiddlewareConfiguration>();

            dependencyManager.RegisterDefaultWebApiAndODataConfiguration();

            dependencyManager.RegisterWebApiMiddleware(webApiDependencyManager =>
            {
                // Enable webApi AuthorizeAttribute
                //webApiDependencyManager.RegisterGlobalWebApiActionFiltersUsing(httpConfiguration =>
                //{
                //    httpConfiguration.Filters.Add(new System.Web.Http.AuthorizeAttribute());
                //});

                webApiDependencyManager.RegisterGlobalWebApiCustomizerUsing(httpConfiguration =>
                {
                    httpConfiguration.EnableSwagger(c =>
                    {
                        EnvironmentAppInfo appInfo = DefaultAppEnvironmentProvider.Current.GetActiveAppEnvironment().AppInfo;
                        c.SingleApiVersion($"v{appInfo.Version}", $"{appInfo.Name}-Api");
                        c.ApplyDefaultApiConfig(httpConfiguration);
                    }).EnableSwaggerUi();
                });

                webApiDependencyManager.RegisterWebApiMiddlewareUsingDefaultConfiguration();
            });

            dependencyManager.RegisterODataMiddleware(odataDependencyManager =>
            {
                // Enable odata AuthorizeAttribute
                //odataDependencyManager.RegisterGlobalWebApiActionFiltersUsing(httpConfiguration =>
                //{
                //    httpConfiguration.Filters.Add(new DefaultODataAuthorizeAttribute());
                //});

                odataDependencyManager.RegisterGlobalWebApiCustomizerUsing(httpConfiguration =>
                {
                    httpConfiguration.EnableSwagger(c =>
                    {
                        EnvironmentAppInfo appInfo = DefaultAppEnvironmentProvider.Current.GetActiveAppEnvironment().AppInfo;
                        c.SingleApiVersion($"v{appInfo.Version}", $"{appInfo.Name}-Api");
                        c.ApplyDefaultODataConfig(httpConfiguration);
                    }).EnableSwaggerUi();
                });

                odataDependencyManager.RegisterODataServiceBuilder<BitODataServiceBuilder>();
                odataDependencyManager.RegisterODataServiceBuilder<SanaapODataServiceBuilder>();
                odataDependencyManager.RegisterWebApiODataMiddlewareUsingDefaultConfiguration();

            });

            dependencyManager.RegisterRepository(typeof(SanaapEfRepository<>).GetTypeInfo());
            dependencyManager.RegisterRepository(typeof(UsersRepository).GetTypeInfo());
            dependencyManager.RegisterRepository(typeof(ExpertsRepository).GetTypeInfo());

            dependencyManager.RegisterEfDbContext<SanaapDbContext>();

            dependencyManager.RegisterDtoEntityMapper();

            dependencyManager.RegisterDtoEntityMapperConfiguration<DefaultDtoEntityMapperConfiguration>();
            dependencyManager.RegisterDtoEntityMapperConfiguration<SanaapDtoEntityMapperConfiguration>();

            dependencyManager.RegisterSingleSignOnServer<SanaapUserService, SanaapClientProvider>();

            dependencyManager.Register<ISmsService, DefaultSmsService>();
        }

        public IEnumerable<IDependenciesManager> GetDependenciesManagers()
        {
            yield return new SanaapDependenciesManager();
        }
    }
}

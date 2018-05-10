using Bit.Core.Implementations;
using Bit.Core.Models;
using Bit.Data.Contracts;
using Bit.Data.EntityFramework.Implementations;
using Bit.Model.Contracts;
using Sanaap.Model;
using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace Sannap.Data
{
    public class SanaapDbContextInitializer : DbMigrationsConfiguration<SanaapDbContext>
    {
        public SanaapDbContextInitializer()
        {
            AutomaticMigrationDataLossAllowed = AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(SanaapDbContext context)
        {
            base.Seed(context);

            if (!context.Set<InsuranceType>().AsNoTracking().Any())
            {
                context.Set<InsuranceType>().AddRange(new[]
                {
                    new InsuranceType
                    {
                        CreatedOn = DateTimeOffset.UtcNow,
                        ModifiedOn = DateTimeOffset.UtcNow,
                        Name = "ثالث",
                        Code = 1
                    },
                    new InsuranceType
                    {
                        CreatedOn = DateTimeOffset.UtcNow,
                        ModifiedOn = DateTimeOffset.UtcNow,
                        Name = "بدنه",
                        Code = 2
                    }
                });
            }
        }
    }

    [DbConfigurationType(typeof(UseDefaultModelStoreDbConfiguration))]
    public class SanaapDbContext : EfDbContextBase
    {
        static SanaapDbContext()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<SanaapDbContext, SanaapDbContextInitializer>());

            using (SanaapDbContext dbContext = new SanaapDbContext())
            {
                dbContext.Database.Initialize(force: true);
            }
        }

        public SanaapDbContext()
            : base(new SqlConnection(DefaultAppEnvironmentsProvider.Current.GetActiveAppEnvironment().GetConfig<string>("SanaapDbConnectionString")), contextOwnsConnection: true)
        {

        }

        public SanaapDbContext(AppEnvironment appEnvironment, IDbConnectionProvider dbConnectionProvider)
            : base(appEnvironment.GetConfig<string>("SanaapDbConnectionString"), dbConnectionProvider)
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            foreach (TypeInfo entityType in typeof(BaseEntity)
                .GetTypeInfo()
                .Assembly
                .GetLoadableExportedTypes()
                .Where(t => t.IsClass && !t.IsAbstract && typeof(IEntity).GetTypeInfo().IsAssignableFrom(t)))
            {
                modelBuilder.RegisterEntityType(entityType);
            }

            base.OnModelCreating(modelBuilder);
        }
    }
}

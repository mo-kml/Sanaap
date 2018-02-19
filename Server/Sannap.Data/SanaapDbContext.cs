using Bit.Core.Contracts;
using Bit.Core.Implementations;
using Bit.Data.Contracts;
using Bit.Data.EntityFramework.Implementations;
using Bit.Model.Contracts;
using Sanaap.Model;
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
            : base(new SqlConnection(DefaultAppEnvironmentProvider.Current.GetActiveAppEnvironment().GetConfig<string>("SanaapDbConnectionString")), contextOwnsConnection: true)
        {

        }

        public SanaapDbContext(IAppEnvironmentProvider appEnvironmentProvider, IDbConnectionProvider dbConnectionProvider)
            : base(appEnvironmentProvider.GetActiveAppEnvironment().GetConfig<string>("SanaapDbConnectionString"), dbConnectionProvider)
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            foreach (TypeInfo entityType in typeof(User)
                .GetTypeInfo()
                .Assembly
                .GetLoadableExportedTypes()
                .Where(t => typeof(IEntity).GetTypeInfo().IsAssignableFrom(t)))
            {
                modelBuilder.RegisterEntityType(entityType);
            }

            base.OnModelCreating(modelBuilder);
        }
    }
}

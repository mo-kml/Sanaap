using Bit.Core.Contracts;
using Bit.Core.Implementations;
using Bit.Core.Models;
using Bit.Data.Contracts;
using Bit.Data.EntityFramework.Implementations;
using Bit.Model.Contracts;
using Sanaap.Model;
using Sannap.Data.Contracts;
using Sannap.Data.Implementations;
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

        public virtual IHashService HashSevice { get; set; } = new DefaultHashService { };

        protected override void Seed(SanaapDbContext context)
        {
            if (!context.Set<User>().AsNoTracking().Any())
            {
                context.Set<User>().Add(new User
                {
                    UserName =
                    "Test",
                    Password = HashSevice.Hash("Test")
                });
            }

            if (!context.Set<City>().AsNoTracking().Any())
            {
                context.Set<City>().Add(new City
                {
                    Name = "تهران",
                    Location = new Location { Lat = 100, Lon = 100 },
                    Version = 1
                });
            }

            base.Seed(context);
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
            foreach (TypeInfo entityType in typeof(User)
                .GetTypeInfo()
                .Assembly
                .GetLoadableExportedTypes()
                .Where(t => t.IsClass && !t.IsAbstract && typeof(IEntity).GetTypeInfo().IsAssignableFrom(t)))
            {
                if (entityType == typeof(BaseEntity).GetTypeInfo())
                    continue;
                modelBuilder.RegisterEntityType(entityType);
            }

            base.OnModelCreating(modelBuilder);
        }
    }
}

using Bit.Core.Implementations;
using Bit.Core.Models;
using Bit.Data.Contracts;
using Bit.Data.EntityFramework.Implementations;
using Bit.Model.Contracts;
using Sanaap.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Dynamic;
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
            if (!context.Set<Company>().AsNoTracking().Any())
            {
                List<Company> companies = new List<Company>{
                new Company { Id = 1, CreatedOn = DateTimeOffset.UtcNow, ModifiedOn = DateTimeOffset.UtcNow, Name = "دانا" },
                new Company { Id = 2, CreatedOn = DateTimeOffset.UtcNow, ModifiedOn = DateTimeOffset.UtcNow, Name = "ایران" },
                new Company { Id = 3, CreatedOn = DateTimeOffset.UtcNow, ModifiedOn = DateTimeOffset.UtcNow, Name = "البرز" },
                new Company { Id = 4, CreatedOn = DateTimeOffset.UtcNow, ModifiedOn = DateTimeOffset.UtcNow, Name = "پارسیان" }
                };

                context.Set<Company>().AddRange(companies);

                List<VehicleKind> vehicleKinds = new List<VehicleKind>
                {
                new VehicleKind { Id = 1, CreatedOn = DateTimeOffset.UtcNow, ModifiedOn = DateTimeOffset.UtcNow, Name = "سمند" },
                new VehicleKind { Id = 2, CreatedOn = DateTimeOffset.UtcNow, ModifiedOn = DateTimeOffset.UtcNow, Name = "پژو 206" },
                new VehicleKind { Id = 3, CreatedOn = DateTimeOffset.UtcNow, ModifiedOn = DateTimeOffset.UtcNow, Name = "پرو 405" },
                new VehicleKind { Id = 4, CreatedOn = DateTimeOffset.UtcNow, ModifiedOn = DateTimeOffset.UtcNow, Name = "پراید" }
                };

                context.Set<VehicleKind>().AddRange(vehicleKinds);

                List<FileType> fileTypes = new List<FileType>
                {
                    new FileType { Id = Guid.NewGuid(), CreatedOn = DateTimeOffset.UtcNow, ModifiedOn = DateTimeOffset.UtcNow, Name = "کارت ملی" },
                    new FileType { Id = Guid.NewGuid(), CreatedOn = DateTimeOffset.UtcNow, ModifiedOn = DateTimeOffset.UtcNow, Name = "شناسنامه" },
                    new FileType { Id = Guid.NewGuid(), CreatedOn = DateTimeOffset.UtcNow, ModifiedOn = DateTimeOffset.UtcNow, Name = "گواهینامه" },
                    new FileType { Id = Guid.NewGuid(), CreatedOn = DateTimeOffset.UtcNow, ModifiedOn = DateTimeOffset.UtcNow, Name = "کارت ماشین" }
                };

                context.Set<FileType>().AddRange(fileTypes);
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

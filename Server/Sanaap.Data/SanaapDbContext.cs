using Bit.Core.Implementations;
using Bit.Core.Models;
using Bit.Data.Contracts;
using Bit.Data.EntityFramework.Implementations;
using Bit.Model.Contracts;
using Sanaap.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Dynamic;
using System.Reflection;

namespace Sanaap.Data
{
    public class SanaapDbContextInitializer : DbMigrationsConfiguration<SanaapDbContext>
    {
        public SanaapDbContextInitializer()
        {
            AutomaticMigrationDataLossAllowed = AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(SanaapDbContext context)
        {
            context.Database.ExecuteSqlCommand("CREATE SEQUENCE RequestNrSequence AS INT START WITH 100 NO CACHE;");

            if (!context.Set<Company>().AsNoTracking().Any())
            {
                context.Set<Company>().AddRange(new List<Company>
                {
                    new Company { Id = 1, CreatedOn = DateTimeOffset.UtcNow, ModifiedOn = DateTimeOffset.UtcNow, Name = "دانا" },
                    new Company { Id = 2, CreatedOn = DateTimeOffset.UtcNow, ModifiedOn = DateTimeOffset.UtcNow, Name = "ایران" },
                    new Company { Id = 3, CreatedOn = DateTimeOffset.UtcNow, ModifiedOn = DateTimeOffset.UtcNow, Name = "البرز" },
                    new Company { Id = 4, CreatedOn = DateTimeOffset.UtcNow, ModifiedOn = DateTimeOffset.UtcNow, Name = "پارسیان" }
                });

                context.Set<VehicleKind>().AddRange(new List<VehicleKind>
                {
                    new VehicleKind { Id = 1, CreatedOn = DateTimeOffset.UtcNow, ModifiedOn = DateTimeOffset.UtcNow, Name = "سمند" },
                    new VehicleKind { Id = 2, CreatedOn = DateTimeOffset.UtcNow, ModifiedOn = DateTimeOffset.UtcNow, Name = "پژو 206" },
                    new VehicleKind { Id = 3, CreatedOn = DateTimeOffset.UtcNow, ModifiedOn = DateTimeOffset.UtcNow, Name = "پژو 405" },
                    new VehicleKind { Id = 4, CreatedOn = DateTimeOffset.UtcNow, ModifiedOn = DateTimeOffset.UtcNow, Name = "پراید" }
                });

                context.Set<EvlRequestFileType>().AddRange(new List<EvlRequestFileType>
                {
                    new EvlRequestFileType { Id = Guid.Parse("9bbd650e-3415-494d-b382-623a0840ab5a"), CreatedOn = DateTimeOffset.UtcNow, ModifiedOn = DateTimeOffset.UtcNow, Name = "کارت ملی" },
                    new EvlRequestFileType { Id = Guid.Parse("733b8551-4982-40e1-a956-d23a6c61f17b"), CreatedOn = DateTimeOffset.UtcNow, ModifiedOn = DateTimeOffset.UtcNow, Name = "شناسنامه" },
                    new EvlRequestFileType { Id = Guid.Parse("65cb06c8-cad9-4d11-90e6-8c6d34f0b84a"), CreatedOn = DateTimeOffset.UtcNow, ModifiedOn = DateTimeOffset.UtcNow, Name = "گواهینامه" },
                    new EvlRequestFileType { Id = Guid.Parse("c804047e-0de2-4fc8-88aa-4df75f367c7a"), CreatedOn = DateTimeOffset.UtcNow, ModifiedOn = DateTimeOffset.UtcNow, Name = "کارت ماشین" }
                });

                context.Set<Customer>()
                    .Add(new Customer { CreatedOn = DateTimeOffset.UtcNow, FirstName = "Yaser", LastName = "Moradi", NationalCode = "1270340050", Mobile = "09033610498", ModifiedOn = DateTimeOffset.UtcNow });
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
            foreach (TypeInfo entityType in Assembly.Load("Sanaap.Model")
                .GetLoadableExportedTypes()
                .Where(t => t.IsClass && !t.IsAbstract && typeof(IEntity).GetTypeInfo().IsAssignableFrom(t)))
            {
                modelBuilder.RegisterEntityType(entityType);
            }

            modelBuilder.Entity<EvlRequestFileType>()
                .Property(evlExpReqFileType => evlExpReqFileType.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);


            base.OnModelCreating(modelBuilder);
        }
    }
}

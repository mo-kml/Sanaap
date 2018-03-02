using Bit.Data.EntityFramework.Implementations;
using Bit.Model.Contracts;
using Sanaap.Model;
using Sanaap.Model.Contracts;
using Sannap.Data.Contracts;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sannap.Data.Implementations
{
    public class SanaapEfRepository<TEntity> : EfRepository<TEntity>, ISanaapRepository<TEntity>
        where TEntity : class, IEntity
    {
        public async override Task<TEntity> AddAsync(TEntity entityToAdd, CancellationToken cancellationToken)
        {
            return await base.AddAsync(entityToAdd, cancellationToken);
        }

        public override void SaveChanges()
        {
            ApplyDefaultValues();
            base.SaveChanges();
        }
        public async override Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            ApplyDefaultValues();
            await base.SaveChangesAsync(cancellationToken);
        }

        protected virtual void ApplyDefaultValues()
        {
            //DbContext.ChangeTracker.DetectChanges();
            //foreach (EntityEntry<IChangeTrackEnableEntity> entry in DbContext.ChangeTracker.Entries()
            //    .Where(entry => entry.Entity is IChangeTrackEnableEntity)
            //    .Where(entry => entry.State == EntityState.Added || entry.State == EntityState.Deleted || entry.State == EntityState.Modified))
            //{
            //    if (entry.State == EntityState.Added)
            //        entry.Entity.InsertDate = DateTimeProvider.GetCurrentUtcDateTime();
            //    else
            //        entry.Entity.EditDate = DateTimeProvider.GetCurrentUtcDateTime();
            //}

            DateTimeOffset Today = DateTimeProvider.GetCurrentUtcDateTime();
            foreach (var entry in DbContext.ChangeTracker.Entries<BaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.AddDate = Today;
                        break;

                    case EntityState.Modified:
                        entry.Entity.EditDate = Today;
                        break;
                }
            }
        }
    }
}

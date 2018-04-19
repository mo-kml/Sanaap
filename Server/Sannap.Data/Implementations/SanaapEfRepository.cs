using Bit.Data.EntityFramework.Implementations;
using Bit.Model.Contracts;
using Sanaap.Model.Contracts;
using Sannap.Data.Contracts;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading;
using System.Threading.Tasks;

namespace Sannap.Data.Implementations
{
    public class SanaapEfRepository<TEntity> : EfRepository<TEntity>, ISanaapRepository<TEntity>
        where TEntity : class, IEntity
    {
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
            DateTimeOffset now = DateTimeProvider.GetCurrentUtcDateTime();

            foreach (DbEntityEntry entry in DbContext.ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        {
                            if (entry.Entity is IChangeTrackEnableEntity addedChangeTrackEnableEntity)
                                addedChangeTrackEnableEntity.CreatedOn = now;
                            break;
                        }
                    case EntityState.Modified:
                        {
                            if (entry.Entity is IChangeTrackEnableEntity modifiedChangeTrackEnableEntity)
                                modifiedChangeTrackEnableEntity.ModifiedOn = now;
                            break;
                        }
                }
            }
        }
    }
}

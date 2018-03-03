using Bit.Data.EntityFramework.Implementations;
using Bit.Model.Contracts;
using Sanaap.Model;
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
            DateTimeOffset Today = DateTimeProvider.GetCurrentUtcDateTime();

            foreach (DbEntityEntry<BaseEntity> entry in DbContext.ChangeTracker.Entries<BaseEntity>())
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

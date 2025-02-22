﻿using Bit.Data.EntityFramework.Implementations;
using Bit.Model.Contracts;
using Sanaap.Data.Contracts;
using Sanaap.Model.Contracts;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading;
using System.Threading.Tasks;

namespace Sanaap.Data.Implementations
{
    public class SanaapEfRepository<TEntity> : EfRepository<TEntity>, ISanaapRepository<TEntity>
        where TEntity : class, IEntity
    {
        protected override void SaveChanges()
        {
            ApplyDefaultValues();
            base.SaveChanges();
        }

        public async Task<int> GetNextSequenceValue()
        {
            DbRawSqlQuery<int> rawQuery = base.DbContext.Database.SqlQuery<int>("SELECT NEXT VALUE FOR RequestNrSequence;");

            return await rawQuery.SingleAsync();
        }

        protected override async Task SaveChangesAsync(CancellationToken cancellationToken)
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
                            {
                                addedChangeTrackEnableEntity.CreatedOn = addedChangeTrackEnableEntity.ModifiedOn = now;
                            }

                            break;
                        }
                    case EntityState.Modified:
                        {
                            if (entry.Entity is IChangeTrackEnableEntity modifiedChangeTrackEnableEntity)
                            {
                                modifiedChangeTrackEnableEntity.ModifiedOn = now;
                            }

                            break;
                        }
                }
            }
        }
    }
}

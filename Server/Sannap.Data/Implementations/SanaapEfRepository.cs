using Bit.Data.EntityFramework.Implementations;
using Bit.Model.Contracts;
using Data.Toolkit;
using Sanaap.Model;
using Sanaap.Model.Contracts;
using Sannap.Data.Contracts;
using System;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
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
            ApplyCorrectYeKe();
            base.SaveChanges();
        }

        public async override Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            ApplyDefaultValues();
            ApplyCorrectYeKe();
            await base.SaveChangesAsync(cancellationToken);
        }

        protected virtual void ApplyDefaultValues()
        {
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

        private void ApplyCorrectYeKe()
        {
            //پیدا کردن موجودیت‌های تغییر کرده
            var changedEntities = DbContext.ChangeTracker.Entries()
                .Where(x => x.State == EntityState.Added || x.State == EntityState.Modified);

            foreach (var item in changedEntities)
            {
                if (item.Entity == null) continue;

                //یافتن خواص قابل تنظیم و رشته‌ای این موجودیت‌ها
                var propertyInfos = item.Entity.GetType().GetProperties(
                    BindingFlags.Public | BindingFlags.Instance
                    ).Where(p => p.CanRead && p.CanWrite && p.PropertyType == typeof(string));

                var pr = new PropertyReflector();

                //اعمال یکپارچگی نهایی
                foreach (var propertyInfo in propertyInfos)
                {
                    var propName = propertyInfo.Name;
                    var val = pr.GetValue(item.Entity, propName);
                    if (val != null)
                    {
                        var newVal = val.ToString().Replace("ي", "ی").Replace("ك", "ک");
                        if (newVal == val.ToString()) continue;
                        pr.SetValue(item.Entity, propName, newVal);
                    }
                }
            }
        }

    }
}

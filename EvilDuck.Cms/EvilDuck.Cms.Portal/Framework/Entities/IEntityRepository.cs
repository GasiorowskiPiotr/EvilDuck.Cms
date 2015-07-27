using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EvilDuck.Cms.Portal.Framework.Entities
{
    public interface IEntityRepository<TEntity, TKey>
    {
        IQueryable<TEntity> AdHocQuery();

        Task<TEntity> GetByKeyAsync(TKey key);

        TEntity GetByKey(TKey key);
    }
}

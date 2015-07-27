using EvilDuck.Cms.Portal.Framework.Logging;
using Microsoft.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace EvilDuck.Cms.Portal.Framework.Entities
{
    public abstract class EntityRepository<TEntity, TKey> : IEntityRepository<TEntity, TKey> where TEntity : Entity<TKey>
    {
        private ApplicationContext _context;
        private ILog _log;
        public EntityRepository(ApplicationContext context, ILog log)
        {
            _context = context;
            _log = log;
            _log.Init(GetType().FullName);
        }

        protected IQueryable<TEntity> Query()
        {
            return _context.Set<TEntity>();
        }

        public IQueryable<TEntity> AdHocQuery()
        {
            return Query();
        }

        public Task<TEntity> GetByKeyAsync(TKey key)
        {
            _log.LogInfo(() => string.Format("Getting entity of type: {0} with key: {1}", typeof(TEntity), key));
            return _context.Set<TEntity>().Where(e => e.Id.Equals(key)).SingleOrDefaultAsync();
        }

        public TEntity GetByKey(TKey key)
        {
            _log.LogInfo(() => string.Format("Getting entity of type: {0} with key: {1}", typeof(TEntity), key));
            return _context.Set<TEntity>().Where(e => e.Id.Equals(key)).SingleOrDefault();
        }
    }
}

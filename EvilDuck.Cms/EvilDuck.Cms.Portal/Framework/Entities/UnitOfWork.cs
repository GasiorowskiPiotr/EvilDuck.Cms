using EvilDuck.Cms.Portal.Framework.Logging;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Relational;
using System;
using System.Data;
using System.Security.Principal;
using System.Threading.Tasks;

namespace EvilDuck.Cms.Portal.Framework.Entities
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationContext _context;
        private ILog _log;
        public UnitOfWork(ApplicationContext context, ILog log)
        {
            _context = context;
            _log = log;
            _log.Init(typeof(UnitOfWork).FullName);
        }

        public void Add<TEntity>(TEntity entity) where TEntity : Entity
        {
            _log.LogInfo(() => String.Format("Adding entity {0}: {1}", typeof(TEntity), entity.ToString()));
            _context.Set<TEntity>().Add(entity);
        }
        public void Attach<TEntity>(TEntity entity) where TEntity : Entity
        {
            _log.LogInfo(() => String.Format("Attaching entity {0}: {1}", typeof(TEntity), entity.ToString()));
            _context.Set<TEntity>().Attach(entity);
        }
        public void Delete<TEntity>(TEntity entity) where TEntity : Entity
        {
            _log.LogInfo(() => String.Format("Deleting entity {0}: {1}", typeof(TEntity), entity.ToString()));
            _context.Set<TEntity>().Remove(entity);
        }
        public void SaveChanges()
        {
            _log.LogInfo(() => String.Format("Saving UnitOfWork changes"));
            _context.SaveChanges();
        }
        public Task SaveChangesAsync()
        {
            _log.LogInfo(() => String.Format("Saving UnitOfWork changes"));
            return _context.SaveChangesAsync();
        }
        public RelationalTransaction BeginTransaction(IsolationLevel isolationLevel)
        {
            _log.LogInfo(() => String.Format("Beginning transaction {0}", isolationLevel));
            return _context.Database.AsRelational().Connection.BeginTransaction(isolationLevel);
        }

        public void SetUser(IIdentity user)
        {
            _log.LogInfo(() => String.Format("Impersonating UnitOfWork as {0}", user.Name));
            _context.SetUser(user);
        }
    }
}

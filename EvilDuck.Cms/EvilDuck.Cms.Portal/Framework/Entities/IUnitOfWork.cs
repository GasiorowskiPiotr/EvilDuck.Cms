using Microsoft.Data.Entity.Storage;
using System.Data;
using System.Security.Principal;
using System.Threading.Tasks;

namespace EvilDuck.Cms.Portal.Framework.Entities
{
    public interface IUnitOfWork
    {
        void Add<TEntity>(TEntity entity) where TEntity : Entity;
        void Attach<TEntity>(TEntity entity) where TEntity : Entity;
        void Delete<TEntity>(TEntity entity) where TEntity : Entity;
        void SaveChanges();
        Task SaveChangesAsync();
        IRelationalTransaction BeginTransaction(IsolationLevel isolationLevel);

        void SetUser(IIdentity user);
    }
}

using EvilDuck.Cms.Portal.Framework.Logging;
using EvilDuck.Cms.Portal.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Threading;

namespace EvilDuck.Cms.Portal.Framework.Entities
{
    public class ApplicationContext : IdentityDbContext<ApplicationUser>
    {
        private ILog _log;
        private IIdentity _user;

        public ApplicationContext(ILog logging)
        {
            logging.Init(GetType().Name);
            _log = logging;
        }

        public void SetUser(IIdentity user)
        {
            _log.LogInfo(() => String.Format("Impersonating {0} as {1}", GetType().Name, user.Name));
            _user = user;
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            HandleWhoColumns();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            HandleWhoColumns();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void HandleWhoColumns()
        {
            var now = DateTime.Now;
            foreach (var entry in this.ChangeTracker.Entries())
            {
                if (entry.State == EntityState.Added)
                {
                    _log.LogInfo(() => String.Format("Creating new entity of type: {0} - {1}", entry.Entity.GetType(), entry.Entity.ToString()));
                    var entity = (Entity)entry.Entity;
                    entity.CreatedBy = _user.Name;
                    entity.CreatedOn = now;
                    entity.LastUpdatedOn = now;
                    entity.LastUpdatedBy = _user.Name;
                }
                else if (entry.State == EntityState.Modified)
                {
                    var entity = (Entity)entry.Entity;
                    _log.LogInfo(() => String.Format("Updating entity of type: {0} with id: {1} - {2}", entry.Entity.GetType(), entity.GetKey(), entity.ToString()));
                    entity.LastUpdatedOn = now;
                    entity.LastUpdatedBy = _user.Name;
                }


            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}

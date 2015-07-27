using EvilDuck.Cms.Portal.Framework.Entities;
using EvilDuck.Cms.Portal.Framework.Logging;
using Microsoft.AspNet.Mvc;
using Microsoft.Data.Entity;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace EvilDuck.Cms.Portal.Framework.Web
{
    public abstract class BaseController<TRepository, TEntity, TKey> : Controller
                where TRepository : IEntityRepository<TEntity, TKey>
                where TEntity : Entity<TKey>, new()
    {
        protected readonly TRepository Repository;
        protected readonly IUnitOfWork UnitOfWork;

        protected readonly ILog Log;

        protected BaseController(IUnitOfWork unitOfWork, TRepository repository, ILog logging)
        {
            Repository = repository;
            UnitOfWork = unitOfWork;
            Log = logging;
            logging.Init(GetType().FullName);
        }

        protected ListResult<TEntity> GetItems(QueryModel queryModel)
        {
            Log.LogInfo(() => String.Format("Getting items with query:", queryModel == null ? String.Empty : queryModel.ToString()));
            int allCount;
            var query = Repository.AdHocQuery();
            query = PreFilter(query);
            if (queryModel != null)
            {
                query = Filter(query, queryModel.FilterField, queryModel.FilterValue,
                    (FilterOper)Enum.Parse(typeof(FilterOper), queryModel.FilterOper));

                allCount = query.Count();

                query = Order(query, queryModel.OrderBy, (OrderDir)queryModel.OrderDir);
                query = Page(query, queryModel.Take, queryModel.Skip);
            }
            else
            {
                allCount = query.Count();

                query = Order(query, String.Empty, OrderDir.Asc);
                query = Page(query, 20, 0);
            }
            throw new Exception("ViewBag!!!");
            ViewBag.AllCount = allCount;
            ViewBag.QueryModel = queryModel;


            var items = query.ToList();

            return new ListResult<TEntity>(query.ToList(), allCount, queryModel);
        }

        protected async Task<ListResult<TEntity>> GetItemsAsync(QueryModel queryModel)
        {
            Log.LogInfo(() => String.Format("Getting items with query:", queryModel == null ? String.Empty : queryModel.ToString()));
            int allCount;
            var query = Repository.AdHocQuery();
            query = PreFilter(query);
            if (queryModel != null)
            {
                query = Filter(query, queryModel.FilterField, queryModel.FilterValue,
                    (FilterOper)Enum.Parse(typeof(FilterOper), queryModel.FilterOper));

                allCount = await query.CountAsync();

                query = Order(query, queryModel.OrderBy, (OrderDir)queryModel.OrderDir);
                query = Page(query, queryModel.Take, queryModel.Skip);
            }
            else
            {
                allCount = await query.CountAsync();

                query = Order(query, String.Empty, OrderDir.Asc);
                query = Page(query, 20, 0);
            }
            
            ViewBag.AllCount = allCount;
            ViewBag.QueryModel = queryModel;

            var items = await query.ToListAsync();

            return new ListResult<TEntity>(items, allCount, queryModel);
        }

        protected bool CreateFrom<TViewModel>(TViewModel viewModel, out TEntity entity) where TViewModel : class
        {
            Log.LogInfo(() => String.Format("Creating entity {0} from {1}:", typeof(TEntity).FullName, viewModel.ToString()));
            CustomValidate(viewModel);
            if (!ModelState.IsValid)
            {
                entity = null;
                return false;
            }

            entity = new TEntity();

            using (var tx = UnitOfWork.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                ViewModelToEntity(viewModel, entity);

                UnitOfWork.Add(entity);
                UnitOfWork.SaveChanges();

                tx.Commit();
            }

            return true;
        }

        protected async Task<TEntity> CreateFromAsync<TViewModel>(TViewModel viewModel) where TViewModel : class
        {
            Log.LogInfo(() => String.Format("Creating entity {0} from {1}:", typeof(TEntity).FullName, viewModel.ToString()));
            CustomValidate(viewModel);
            if (!ModelState.IsValid)
            {
                return await Task.FromResult<TEntity>(null);
            }

            var entity = new TEntity();

            using (var tx = UnitOfWork.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                ViewModelToEntity(viewModel, entity);

                UnitOfWork.Add(entity);
                await UnitOfWork.SaveChangesAsync();

                tx.Commit();
            }

            return entity;
        }

        protected async Task<TEntity> UpdateFromAsync<TViewModel>(TKey entityKey, TViewModel viewModel) where TViewModel : class
        {
            CustomValidate(viewModel);
            if (!ModelState.IsValid)
            {
                return null;
            }

            using (var tx = UnitOfWork.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                var entity = await Repository.GetByKeyAsync(entityKey);
                if (entity == null)
                {
                    ModelState.AddModelError("Entity", "Podana encja nie istnieje w bazie");
                    return null;
                }

                ViewModelToEntity(viewModel, entity);
                await UnitOfWork.SaveChangesAsync();

                tx.Commit();

                return entity;
            }
        }

        protected async Task RemoveAsync(TKey key)
        {
            var entity = await Repository.GetByKeyAsync(key);
            if (entity == null)
            {
                return;
            }

            using (var tx = UnitOfWork.BeginTransaction(IsolationLevel.ReadCommitted))
            {

                DisattachReferences(entity);
                UnitOfWork.Delete(entity);
                await UnitOfWork.SaveChangesAsync();

                tx.Commit();
            }
        }

        protected TEntity GetItem(TKey key)
        {
            return Repository.GetByKey(key);
        }

        protected async Task<TEntity> GetItemAsync(TKey key)
        {
            return await Repository.GetByKeyAsync(key);
        }

        protected virtual void DisattachReferences(TEntity entity)
        {

        }

        protected virtual void CustomValidate<TViewModel>(TViewModel viewModel)
        {

        }

        protected virtual void ViewModelToEntity<TViewModel>(TViewModel viewModel, TEntity entity) where TViewModel : class
        {
            throw new NotImplementedException("This must be implemented when Creating / Updating entities.");
        }

        protected virtual IQueryable<TEntity> PreFilter(IQueryable<TEntity> query)
        {
            return query;
        }

        protected virtual IQueryable<TEntity> Filter(IQueryable<TEntity> query, string filterField, string filterValue, FilterOper filterOper)
        {
            return query;
        }

        protected virtual IQueryable<TEntity> Order(IQueryable<TEntity> query, string orderBy, OrderDir orderDir)
        {
            return query.OrderBy(e => e.Id);
        }

        protected virtual IQueryable<TEntity> Page(IQueryable<TEntity> query, int take, int skip)
        {
            return query.Skip(skip).Take(take);
        }
    }
}

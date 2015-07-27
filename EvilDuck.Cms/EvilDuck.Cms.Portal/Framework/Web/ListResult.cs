using EvilDuck.Cms.Portal.Framework.Entities;
using System.Collections.Generic;

namespace EvilDuck.Cms.Portal.Framework.Web
{
    public class ListResult
    {
        public ListResult(int allCount, QueryModel queryModel)
        {
            AllCount = allCount;
            QueryModel = queryModel;
        }

        public int AllCount { get; private set; }
        public QueryModel QueryModel { get; private set; }
    }

    public class ListResult<TEntity> : ListResult where TEntity : Entity
    {
        public IEnumerable<TEntity> Entities { get; private set; }

        public ListResult(IEnumerable<TEntity> entities, int allCount, QueryModel queryModel) : base(allCount, queryModel)
        {
            Entities = entities;
        }
    }
}

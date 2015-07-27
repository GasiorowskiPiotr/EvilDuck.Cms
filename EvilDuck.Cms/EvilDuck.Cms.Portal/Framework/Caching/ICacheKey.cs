using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EvilDuck.Cms.Portal.Framework.Caching
{
    public interface ICacheKey
    {
        string GetStringKey();
    }
}

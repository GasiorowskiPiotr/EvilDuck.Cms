using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EvilDuck.Cms.Portal.Framework.Logging
{
    public interface ILog
    {

        void LogError(Func<string> func);
        void LogWarn(Func<string> func);
        void LogInfo(Func<string> func);

        void Init(string name);
    }
}

using EvilDuck.Cms.Portal.Framework.Entities;
using Microsoft.AspNet.Http;
using System;
using System.Threading.Tasks;

namespace EvilDuck.Cms.Portal.Framework
{
    public abstract class Installer
    {
        public async Task PerformInstallation(ApplicationContext context)
        {
            if(await CanPerform(context))
            {
                var result = await Install(context);
                result.ThrowIfError();
            }
        }

        protected abstract Task<bool> CanPerform(ApplicationContext context);
        protected abstract Task<Result> Install(ApplicationContext context);

        public void Initialize(HttpRequest request)
        {
            ApplicationServices = request.HttpContext.ApplicationServices;
            Request = request;
        }

        protected IServiceProvider ApplicationServices
        {
            get; private set;
        }

        protected HttpRequest Request
        {
            get; private set;
        }
    }
}

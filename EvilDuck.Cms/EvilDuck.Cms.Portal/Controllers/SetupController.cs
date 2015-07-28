using EvilDuck.Cms.Portal.Framework;
using EvilDuck.Cms.Portal.Framework.Entities;
using EvilDuck.Cms.Portal.Framework.Utils;
using EvilDuck.Cms.Portal.Properties;
using Microsoft.AspNet.Mvc;
using Microsoft.Framework.OptionsModel;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EvilDuck.Cms.Portal.Controllers
{
    public class SetupController : Controller
    {
        private AppSettings _settings;
        private ApplicationContext _appContext;
        private IUnitOfWork _unitOfWork;

        public SetupController(IConfigureOptions<AppSettings> appSettings, ApplicationContext context, IUnitOfWork unitOfWork)
        {
            _settings = new AppSettings();
            appSettings.Configure(_settings);
            
            _appContext = context;
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Installers()
        {
            using(var tx = _unitOfWork.BeginTransaction(System.Data.IsolationLevel.Serializable))
            {
                var installers = _settings
                .Installers
                .Select(it => Type.GetType(it))
                .Select(t => (Installer)Activator.CreateInstance(t));
                
                foreach(var installer in installers)
                {
                    installer.Initialize(Request);
                    var t = installer.PerformInstallation(_appContext);
                    t.Wait();
                }
                
                

                await _appContext.SaveChangesAsync();
                tx.Commit();

                return Json("OK");
            }

            
        }

    }
}

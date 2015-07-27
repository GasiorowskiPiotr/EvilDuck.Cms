using EvilDuck.Cms.Portal.Framework.Entities;
using EvilDuck.Cms.Portal.Framework.Logging;
using EvilDuck.Cms.Portal.Framework.Utils;
using EvilDuck.Cms.Portal.Models.Admin.Roles;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Mvc;
using Microsoft.Data.Entity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EvilDuck.Cms.Portal.Controllers.Admin
{
    [Authorize]
    [Route("admin/[controller]")]
    public class RolesController : Controller
    {
        private ApplicationContext _context;

        private RoleManager<IdentityRole> _rolesManager;

        private ILog _logging;

        public RolesController(RoleManager<IdentityRole> roleManager, ApplicationContext context, ILog logging)
        {
            this._rolesManager = roleManager;
            this._context = context;
            this._logging = logging;
            this._logging.Init(typeof(RolesController).FullName);
        }

        [Route("")]
        public IActionResult Index()
        {
            _logging.LogInfo(() => "Getting roles from Database.");

            var roles = _context.Roles.Select(RoleListViewModel.FromEntity).ToList();
            return View(roles);
        }

        [Route("[action]")]
        public IActionResult Add()
        {
            _logging.LogInfo(() => "Generating view for adding roles.");

            return View(new AddRoleViewModel());
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult> Add(AddRoleViewModel vm)
        {

            _logging.LogInfo(() => String.Format("Starting adding new role: {0}", vm.Name));

            if (!ModelState.IsValid)
            {
                _logging.LogWarn(() => String.Format("Not all the fileds are valid. Model state: {0}", String.Join(";", ModelState.Select(e => String.Format("{0} = {1}", e.Key, String.Join(",", e.Value.Errors.Select(e2 => e2.ErrorMessage)))))));
                return View(vm);
            }

            var result = await _rolesManager.CreateAsync(new IdentityRole
            {
                Name = vm.Name
            });
            if (result.Succeeded)
            {
                _logging.LogInfo(() => String.Format("Role: {0} created.", vm.Name));

                return RedirectToAction("Index");
            }
            result.Errors.Do(e => ModelState.AddModelError(string.Empty, e.Description));
            _logging.LogWarn(() => String.Format("Could not create role. Model state: {0}", String.Join(";", ModelState.Select(e => String.Format("{0} = {1}", e.Key, String.Join(",", e.Value.Errors.Select(e2 => e2.ErrorMessage)))))));

            return View(vm);
        }

        [Route("[action]")]
        public async Task<ActionResult> Remove(string id)
        {
            _logging.LogInfo(() => String.Format("Removing role with ID: {0}.", id));

            var role = await _context.Roles.SingleOrDefaultAsync(r => r.Id == id);
            if (role != null)
            {
                _context.Roles.Remove(role);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
    }
}

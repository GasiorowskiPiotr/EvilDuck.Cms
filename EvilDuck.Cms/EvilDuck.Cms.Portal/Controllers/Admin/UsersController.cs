using EvilDuck.Cms.Portal.Framework.Entities;
using EvilDuck.Cms.Portal.Framework.Logging;
using EvilDuck.Cms.Portal.Framework.Utils;
using EvilDuck.Cms.Portal.Models;
using EvilDuck.Cms.Portal.Models.Admin.Users;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Mvc;
using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EvilDuck.Cms.Portal.Controllers.Admin
{
    //[Authorize]
    [Route("admin/[controller]")]
    public class UsersController : Controller
    {
        private ApplicationContext _context;
        private UserManager<ApplicationUser> _userManager;

        private ILog _logger;

        public UsersController(UserManager<ApplicationUser> userManager, ApplicationContext context, ILog log)
        {
            _context = context;
            _userManager = userManager;
            _logger = log;
            _logger.Init(GetType().ToString());
        }

        [Route("")]
        public IActionResult Index()
        {
            _logger.LogInfo(() => "Getting all the users from DB.");

            var users = _context.Users.Select(UserListViewModel.FromEntity).ToList();
            return View(users);
        }

        [Route("[action]")]
        public IActionResult Add()
        {
            _logger.LogInfo(() => "Generating view for adding users.");
            return View(new AddUserViewModel());
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult> Add(AddUserViewModel vm)
        {
            _logger.LogInfo(() => "Starting adding new user.");

            if (!ModelState.IsValid)
            {
                _logger.LogWarn(() => String.Format("Not all the fileds are valid. Model state: {0}", String.Join(";", ModelState.Select(e => String.Format("{0} = {1}", e.Key, String.Join(",", e.Value.Errors.Select(e2 => e2.ErrorMessage)))))));
                

                return View(vm);
            }

            var result = await _userManager.CreateAsync(new ApplicationUser
            {
                Email = vm.Email,
                UserName = vm.Email,
            }, vm.Password);

            if (result.Succeeded)
            {
                _logger.LogInfo(() => String.Format("User: {0} created.", vm.Email));
                return RedirectToAction("Index");
            }
            result.Errors.Do(e => ModelState.AddModelError(string.Empty, e.Description));
            _logger.LogWarn(() => String.Format("Could not create user. Model state: {0}", String.Join(";", ModelState.Select(e => String.Format("{0} = {1}", e.Key, String.Join(",", e.Value.Errors.Select(e2 => e2.ErrorMessage)))))));
            
            return View(vm);
        }

        [Route("[action]")]
        public async Task<ActionResult> Remove(string id)
        {
            _logger.LogInfo(() => String.Format("Removing user with ID: {0}.", id));

            var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        [Route("[action]")]
        public ActionResult ChangePassword(string id)
        {
            return View(new ChangeUserPasswordViewModel
            {
                UserId = id
            });
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult> ChangePassword(ChangeUserPasswordViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var user = await _userManager.FindByIdAsync(vm.UserId);
            if (user == null)
            {
                ModelState.AddModelError(String.Empty, "Użytkownik nie istnieje w bazie danych.");
                return View(vm);
            }

            var result = await _userManager.ChangePasswordAsync(user, vm.OldPassword, vm.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            result.Errors.Do(e => ModelState.AddModelError(String.Empty, new Exception(e.Description)));
            return View(vm);
        }
    }
}

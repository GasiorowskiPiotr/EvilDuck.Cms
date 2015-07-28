using EvilDuck.Cms.Portal.Framework;
using System.Linq;
using EvilDuck.Cms.Portal.Framework.Entities;
using EvilDuck.Cms.Portal.Models;
using Microsoft.AspNet.Identity;
using System;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;
using System.Threading.Tasks;

namespace EvilDuck.Cms.Portal.Installers
{
    public class RolesInstaller : Installer
    {
        protected override async Task<bool> CanPerform(ApplicationContext context)
        {
            return await context.Roles.AnyAsync(r => r.Name == "Administrator");
        }

        protected override async Task<Result> Install(ApplicationContext context)
        { 
            var role = new IdentityRole();
            role.Name = "Administrator";

            context.Roles.Add(role);

            return await Task.FromResult(Result.Success());
        }
    }

    public class SecurityInstaller : Installer
    {
        protected override async Task<bool> CanPerform(ApplicationContext context)
        {
            return await context.Users.Where(u => u.Email == "admin@evilduck").AnyAsync();
        }

        protected override async Task<Result> Install(ApplicationContext context)
        {
            try
            {
                var user = new ApplicationUser();
                user.Email = "admin@evilduck";
                user.UserName = "admin@evilduck";

                var userManager = (UserManager<ApplicationUser>)ApplicationServices.GetService(typeof(UserManager<ApplicationUser>));
                var result = await userManager.CreateAsync(user, "!QAZxsw2#");

                if(!result.Succeeded)
                {
                    return Result.Failure(String.Join(",", result.Errors.Select(e => e.Description)));
                }

                result = await userManager.AddToRoleAsync(user, "Administrator");
                if(!result.Succeeded)
                {
                    return Result.Failure(String.Join(",", result.Errors.Select(e => e.Description)));
                }
                return Result.Success();
            }
            catch(Exception ex)
            {
                return Result.Failure("Błąd w czasie dodawanie administratora", Request, ex);
            }
       }
    }
}

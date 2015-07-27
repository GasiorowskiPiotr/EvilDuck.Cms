using Microsoft.AspNet.Identity.EntityFramework;

namespace EvilDuck.Cms.Portal.Models.Admin.Roles
{
    public class RoleListViewModel
    {
        public string Name { get; set; }
        public string Id { get; set; }

        public static RoleListViewModel FromEntity(IdentityRole role)
        {
            return new RoleListViewModel
            {
                Id = role.Id,
                Name = role.Name,
            };
        }
    }
}

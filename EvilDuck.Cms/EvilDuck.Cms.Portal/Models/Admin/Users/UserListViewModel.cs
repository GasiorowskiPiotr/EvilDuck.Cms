namespace EvilDuck.Cms.Portal.Models.Admin.Users
{
    public class UserListViewModel
    {
        public string Username { get; set; }
        public string Id { get; set; }

        public static UserListViewModel FromEntity(ApplicationUser user)
        {
            return new UserListViewModel
            {
                Id = user.Id,
                Username = user.Email,
            };
        }
    }
}

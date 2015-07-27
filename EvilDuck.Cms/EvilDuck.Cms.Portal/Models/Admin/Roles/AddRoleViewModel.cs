using System.ComponentModel.DataAnnotations;

namespace EvilDuck.Cms.Portal.Models.Admin.Roles
{
    public class AddRoleViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Nazwa roli jest wymagana")]
        [MaxLength(64, ErrorMessage = "Nazwa roli jest za długa")]
        [Display(Name = "Nazwa roli")]
        public string Name { get; set; }
    }
}

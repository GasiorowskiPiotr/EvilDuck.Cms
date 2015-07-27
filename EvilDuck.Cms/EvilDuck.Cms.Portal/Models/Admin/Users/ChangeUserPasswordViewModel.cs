using System.ComponentModel.DataAnnotations;

namespace EvilDuck.Cms.Portal.Models.Admin.Users
{
    public class ChangeUserPasswordViewModel
    {
        public string UserId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Haslo jest wymagane")]
        [DataType(DataType.Password)]
        [MaxLength(64, ErrorMessage = "Hasło jest za długie")]
        [Display(Name = "Stare hasło")]
        public string OldPassword { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Haslo jest wymagane")]
        [DataType(DataType.Password)]
        [MaxLength(64, ErrorMessage = "Hasło jest za długie")]
        [Display(Name = "Nowe hasło")]
        public string Password { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Proszę potwierdzić hasło")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        [DataType(DataType.Password)]
        [MaxLength(64, ErrorMessage = "Hasło jest za długie")]
        [Display(Name = "Powtórz nowe hasło")]
        public string ConfirmPassword { get; set; }
    }
}

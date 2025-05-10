using System.ComponentModel.DataAnnotations;

namespace NoteKeeperPro.Web.ViewModels.Identity
{
    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Confirm Password is required.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        [Display(Name = "Confirm Password")]
        public string ConfirmedPassword { get; set; } = null!;

        // إضافة الخصائص المفقودة
        public string Email { get; set; } = null!;
        public string Token { get; set; } = null!;
    }
}

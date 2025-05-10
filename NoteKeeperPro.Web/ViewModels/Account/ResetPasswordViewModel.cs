using System.ComponentModel.DataAnnotations;

namespace NoteKeeperPro.Web.ViewModels.Account;

public class ResetPasswordViewModel
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$",
        ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character")]
    public string Password { get; set; } = string.Empty;

    [DataType(DataType.Password)]
    [Display(Name = "Confirm Password")]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; } = string.Empty;

    [Required]
    public string Token { get; set; } = string.Empty;
}
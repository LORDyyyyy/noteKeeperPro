using System.ComponentModel.DataAnnotations;

namespace NoteKeeperPro.Web.ViewModels.Identity
{
    public class RegisterViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "FName is required.")]
        [StringLength(100, ErrorMessage = "Name must be at most 100 characters.")]
        public string FName { get; set; } = null!;

        [Required(ErrorMessage = "LName is required.")]
        [StringLength(100, ErrorMessage = "Name must be at most 100 characters.")]
        public string LName { get; set; } = null!;



        public bool IsActive { get; set; }

      
    
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Confirm Password is required.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        [Display(Name = "Confirm Password")]
        public string ConfirmedPassword { get; set; } = null!;

        public bool IsAgree { get; set; } = false;

    }

}

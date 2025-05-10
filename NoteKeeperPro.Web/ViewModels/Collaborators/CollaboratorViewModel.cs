using System.ComponentModel.DataAnnotations;

namespace NoteKeeperPro.Web.ViewModels.Collaborators
{
    public class CollaboratorViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Note ID is required.")]
        public int NoteId { get; set; }

        public string NoteTitle { get; set; } = string.Empty;

        [Required(ErrorMessage = "User ID is required.")]
        public string UserId { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Permission type is required.")]
        [StringLength(20, ErrorMessage = "Permission type must be 20 characters or fewer.")]
        public string? PermissionType { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}

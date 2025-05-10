using System.ComponentModel.DataAnnotations;

namespace NoteKeeperPro.Web.ViewModels.Tags
{
    public class TagViewModel
    {
        public int Id { get; set; }

        // Tag name
        [Required(ErrorMessage = "Tag name is required.")]
        [StringLength(100, ErrorMessage = "Tag name must be less than 100 characters.")]
        public string Name { get; set; } = string.Empty;

        // Soft delete flag
        public bool IsDeleted { get; set; } = false;

        // List of notes associated with this tag (we will only store the IDs here)
        public ICollection<int> NoteIds { get; set; } = new List<int>();
    }
}

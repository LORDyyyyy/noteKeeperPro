using System.ComponentModel.DataAnnotations;

namespace NoteKeeperPro.Web.ViewModels.NotesInfo
{
    public class NoteInfoViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Note ID is required.")]
        public int NoteId { get; set; }

        [Required(ErrorMessage = "Creation date is required.")]
        public DateTime CreatedAt { get; set; }

        [Required(ErrorMessage = "Last modification date is required.")]
        public DateTime LastModifiedAt { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Word count must be a non-negative number.")]
        public int WordCount { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Character count must be a non-negative number.")]
        public int CharacterCount { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}

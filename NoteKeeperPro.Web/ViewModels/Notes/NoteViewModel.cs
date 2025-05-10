using System.ComponentModel.DataAnnotations;
using NoteKeeperPro.Domain.Entities.M_M_RelationShips;
using NoteKeeperPro.Web.ViewModels.Collaborators;

namespace NoteKeeperPro.Web.ViewModels.Notes
{
    public class NoteViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(100, ErrorMessage = "Title must be between 3 and 100 characters.", MinimumLength = 3)]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Content is required.")]
        public string Content { get; set; } = string.Empty;

        public string NoteInfo { get; set; } = string.Empty;

        public List<CollaboratorViewModel> Collaborators { get; set; } = new();
        public List<string> TagNames { get; set; } = new();
    }

}

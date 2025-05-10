using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoteKeeperPro.Domain.Entities.ApplicationUsers;
using NoteKeeperPro.Domain.Entities.Collaborators;
using NoteKeeperPro.Domain.Entities.M_M_RelationShips;
using NoteKeeperPro.Domain.Entities.NotesInfo;
using NoteKeeperPro.Domain.Entities.Tags;
using NoteKeeperPro.Infrastructure.Identity;
namespace NoteKeeperPro.Domain.Entities.Notes
{
    public class Note
    {
        public int Id { get; set; }

        public required string Title { get; set; }
        public required string Content { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public bool IsDeleted { get; set; } = false;

        // FK to the user who owns this note
        public string OwnerId { get; set; } = null!;

        // Navigation to the owner user
        public ApplicationUser Owner { get; set; } = null!;

        // Navigation to info 
        public NoteInfo NoteInfo { get; set; } = null!;

        // M-M relationShip with Colaborator with NoteCollaborator Middle Table
        public ICollection<NoteCollaborator> NoteCollaborators { get; set; } = new HashSet<NoteCollaborator>();

        // M-M relationShip with Tag with Middle Table
        public ICollection<NoteTag> NoteTags { get; set; } = new HashSet<NoteTag>();
    }

}

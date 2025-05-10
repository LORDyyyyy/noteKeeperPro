using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoteKeeperPro.Domain.Entities.ApplicationUsers;
using NoteKeeperPro.Domain.Entities.M_M_RelationShips;
using NoteKeeperPro.Domain.Entities.Notes;
using NoteKeeperPro.Infrastructure.Identity;

namespace NoteKeeperPro.Domain.Entities.Collaborators
{
    public enum PermissionType
    {
        Edit = 1,
        View = 2,
    }
    public class Collaborator
    {
        public int Id { get; set; }

        // FK to the shared note
        public int NoteId { get; set; }

        public string UserName { get; set; }

        // Navigation to the note being shared
        public Note Note { get; set; } = null!;

        // FK to the collaborating user
        public string UserId { get; set; } = null!;

        // Navigation to the collaborating user
        public ApplicationUser User { get; set; } = null!;

        // Access level of the collaborator (Read, Write, etc.)
        public PermissionType PermissionType { get; set; }

        // Soft delete flag for collaboration (e.g., revoked access)
        public bool IsDeleted { get; set; } = false;

        // علاقة مع Note عبر NoteCollaborator
        public ICollection<NoteCollaborator> NoteCollaborators { get; set; } = new HashSet<NoteCollaborator>();
    }


}


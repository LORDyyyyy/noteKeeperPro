using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoteKeeperPro.Domain.Entities.ApplicationUsers;
using NoteKeeperPro.Domain.Entities.Collaborators;
using NoteKeeperPro.Domain.Entities.Notes;

namespace NoteKeeperPro.Application.Dtos.Collaborators
{
    public class CollaboratorDetailsToReturnDto
    {
        public int Id { get; set; }
        public int NoteId { get; set; }

        public string UserName { get; set; }

       public Note Note { get; set; } = null!;

        public string UserId { get; set; } = null!;

       public ApplicationUser User { get; set; } = null!;

        public PermissionType PermissionType { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}

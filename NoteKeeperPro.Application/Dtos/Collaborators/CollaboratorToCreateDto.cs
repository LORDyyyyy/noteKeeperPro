using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoteKeeperPro.Domain.Entities.Collaborators;

namespace NoteKeeperPro.Application.Dtos.Collaborators
{
    public class CollaboratorToCreateDto
    {
        public int NoteId { get; set; }
        public string UserName { get; set; }

        public string UserId { get; set; } = null!;
        public PermissionType PermissionType { get; set; }
    }

}

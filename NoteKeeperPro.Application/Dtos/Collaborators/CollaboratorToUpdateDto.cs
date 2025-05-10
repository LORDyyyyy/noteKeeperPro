using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoteKeeperPro.Domain.Entities.Collaborators;

namespace NoteKeeperPro.Application.Dtos.Collaborators
{
    public class CollaboratorToUpdateDto
    {
        public int Id { get; set; }
        public string UserName { get; set; } = null!; // من ApplicationUser.UserName
        public PermissionType PermissionType { get; set; }
    }

}

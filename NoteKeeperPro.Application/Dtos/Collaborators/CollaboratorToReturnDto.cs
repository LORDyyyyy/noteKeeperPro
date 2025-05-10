using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoteKeeperPro.Domain.Entities.Collaborators;

namespace NoteKeeperPro.Application.Dtos.Collaborators
{
   public class CollaboratorToReturnDto
{
    public int Id { get; set; }
    public int NoteId { get; set; }
    public string UserId { get; set; } = null!;
    public string UserName { get; set; } = null!; // من ApplicationUser.UserName
    public PermissionType PermissionType { get; set; }
    public bool IsDeleted { get; set; }
}

}

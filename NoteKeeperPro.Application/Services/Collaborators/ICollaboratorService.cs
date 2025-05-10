using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoteKeeperPro.Application.Dtos.Collaborators;
using NoteKeeperPro.Domain.Entities.Collaborators;

namespace NoteKeeperPro.Application.Services.Collaborators
{
    public interface ICollaboratorService
    {
        IEnumerable<CollaboratorToReturnDto> GetAllCollaborators();
        CollaboratorDetailsToReturnDto? GetCollaboratorById(int id);
        int CreateCollaborator(CollaboratorToCreateDto collaborator);
        int UpdateCollaborator(CollaboratorToUpdateDto collaborator);
        bool DeleteCollaborator(int id);
    }
}

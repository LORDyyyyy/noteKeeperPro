using NoteKeeperPro.Application.Dtos.Collaborators;
using NoteKeeperPro.Domain.Entities.Collaborators;
using NoteKeeperPro.Infrastructure.Presistance.Repositories.Collaborators;

namespace NoteKeeperPro.Application.Services.Collaborators
{
    public class CollaboratorService : ICollaboratorService
    {
        private readonly ICollaboratorRepository _collaboratorRepository;

        public CollaboratorService(ICollaboratorRepository collaboratorRepository)
        {
            _collaboratorRepository = collaboratorRepository;
        }

        public int CreateCollaborator(CollaboratorToCreateDto collaboratorDto)
        {
            var collaborator = new Collaborator
            {
                NoteId = collaboratorDto.NoteId,
                UserId = collaboratorDto.UserId,
                PermissionType = collaboratorDto.PermissionType,
                IsDeleted = false
            };

            return _collaboratorRepository.AddCollaborator(collaborator);
        }

        public bool DeleteCollaborator(int id)
        {
            var collaborator = _collaboratorRepository.GetById(id);

            if (collaborator != null)
            {
                collaborator.IsDeleted = true;
                return _collaboratorRepository.UpdateCollaborator(collaborator) > 0;
            }

            return false;
        }

        // Get all Collaborators
        public IEnumerable<CollaboratorToReturnDto> GetAllCollaborators()
        {
            var collaborators = _collaboratorRepository.GetAllQuarable()
                .Where(c => c.IsDeleted == false)
                .Select(c => new CollaboratorToReturnDto
                {
                    Id = c.Id,
                    NoteId = c.NoteId,
                    UserId = c.UserId,
                    UserName = c.User.UserName, // Assuming User is a navigation property
                    PermissionType = c.PermissionType,
                    IsDeleted = c.IsDeleted
                })
                .ToList();

            return collaborators;
        }

        // Get a specific Collaborator by ID
        public CollaboratorDetailsToReturnDto? GetCollaboratorById(int id)
        {
            var collaborator = _collaboratorRepository.GetById(id);

            if (collaborator != null)
            {
                return new CollaboratorDetailsToReturnDto
                {
                    Id = collaborator.Id,
                    NoteId = collaborator.NoteId,
                    Note = collaborator.Note, // Assuming Note is a navigation property
                    UserId = collaborator.UserId,
                    User = collaborator.User, // Assuming User is a navigation property
                    PermissionType = collaborator.PermissionType,
                    IsDeleted = collaborator.IsDeleted
                };
            }

            return null;
        }

        // Update a Collaborator's Permission
        public int UpdateCollaborator(CollaboratorToUpdateDto collaboratorDto)
        {
            var collaborator = _collaboratorRepository.GetById(collaboratorDto.Id);

            if (collaborator != null)
            {
                collaborator.PermissionType = collaboratorDto.PermissionType;
                return _collaboratorRepository.UpdateCollaborator(collaborator);
            }

            return 0; // Return 0 if collaborator is not found
        }
    }
}
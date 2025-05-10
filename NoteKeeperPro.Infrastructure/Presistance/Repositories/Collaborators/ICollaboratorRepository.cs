using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoteKeeperPro.Domain.Entities.Collaborators;

namespace NoteKeeperPro.Infrastructure.Presistance.Repositories.Collaborators
{
    public interface ICollaboratorRepository
    {
        IEnumerable<Collaborator> GetAll(bool AsNoTracking = true);
        IQueryable<Collaborator> GetAllQuarable(bool AsNoTracking = true);
        Collaborator GetById(int id);
        int AddCollaborator(Collaborator collaborator);
        int UpdateCollaborator(Collaborator collaborator);
        int DeleteCollaborator(Collaborator collaborator);
        Task<Collaborator> GetOrCreateCollaboratorAsync(string email);
        Task<Collaborator> GetByEmailAsync(string email);
        Task<Collaborator> GetByIdAsync(int id);
    }
}

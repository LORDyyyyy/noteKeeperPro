using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoteKeeperPro.Domain.Entities.ApplicationUsers;
using NoteKeeperPro.Domain.Entities.Collaborators;
using NoteKeeperPro.Infrastructure.Identity;

namespace NoteKeeperPro.Infrastructure.Presistance.Repositories.ApplicationUsers
{
    public interface IApplicationUserRepository
    {
        Task<IEnumerable<ApplicationUser>> GetAllAsync(bool asNoTracking = true); 
        Task<IQueryable<ApplicationUser>> GetAllQueryableAsync(bool asNoTracking = true); 
        Task<ApplicationUser> GetByIdAsync(string userId);
        Task<ApplicationUser> GetByEmailAsync(string email);
        Task<ApplicationUser> CreateAsync(ApplicationUser user);
        Task<ApplicationUser> UpdateAsync(ApplicationUser user);
        Task DeleteAsync(string userId);
    }

}

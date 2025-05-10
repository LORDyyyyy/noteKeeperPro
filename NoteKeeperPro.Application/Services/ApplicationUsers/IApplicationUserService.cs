using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoteKeeperPro.Domain.Entities.ApplicationUsers;

namespace NoteKeeperPro.Application.Services.ApplicationUsers
{
    public interface IApplicationUserService
    {
        Task<ApplicationUser> GetUserByIdAsync(string userId);
        Task<ApplicationUser> GetUserByEmailAsync(string email);
        Task<ApplicationUser> CreateUserAsync(ApplicationUser user);
        Task<ApplicationUser> UpdateUserAsync(ApplicationUser user);
        Task DeleteUserAsync(string userId);
    }
}

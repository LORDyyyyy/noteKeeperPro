using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoteKeeperPro.Domain.Entities.ApplicationUsers;
using NoteKeeperPro.Infrastructure.Presistance.Repositories.ApplicationUsers;

namespace NoteKeeperPro.Application.Services.ApplicationUsers
{
    public class ApplicationUserService : IApplicationUserService
    {
        private readonly IApplicationUserRepository _repository;

        public ApplicationUserService(IApplicationUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<ApplicationUser> GetUserByIdAsync(string userId)
        {
            var user = await _repository.GetByIdAsync(userId);
            if (user == null)
                throw new Exception("User not found");
            return user;
        }

        public async Task<ApplicationUser> GetUserByEmailAsync(string email)
        {
            var user = await _repository.GetByEmailAsync(email);
            if (user == null)
                throw new Exception("User not found");
            return user;
        }

        public async Task<ApplicationUser> CreateUserAsync(ApplicationUser user)
        {
            return await _repository.CreateAsync(user);
        }

        public async Task<ApplicationUser> UpdateUserAsync(ApplicationUser user)
        {
            return await _repository.UpdateAsync(user);
        }

        public async Task DeleteUserAsync(string userId)
        {
            await _repository.DeleteAsync(userId);
        }
    }
}
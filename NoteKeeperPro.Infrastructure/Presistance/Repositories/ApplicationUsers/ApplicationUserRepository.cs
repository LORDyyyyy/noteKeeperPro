using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NoteKeeperPro.Domain.Entities.ApplicationUsers;
using NoteKeeperPro.Infrastructure.Presistance.Data;

namespace NoteKeeperPro.Infrastructure.Presistance.Repositories.ApplicationUsers
{
    public class ApplicationUserRepository : IApplicationUserRepository
    {
        private readonly ApplicationDbContext _context;

        public ApplicationUserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ApplicationUser> GetByIdAsync(string userId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<ApplicationUser> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<ApplicationUser> CreateAsync(ApplicationUser user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<ApplicationUser> UpdateAsync(ApplicationUser user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task DeleteAsync(string userId)
        {
            var user = await GetByIdAsync(userId);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllAsync(bool asNoTracking = true)
        {
            if (asNoTracking)
            {
                // Detached (No tracking)
                return await _context.Users.AsNoTracking().ToListAsync();
            }

            // Unchanged (Tracking enabled)
            return await _context.Users.ToListAsync();
        }

        public Task<IQueryable<ApplicationUser>> GetAllQueryableAsync(bool asNoTracking = true)
        {
            var query = _context.Users.AsQueryable();

            if (asNoTracking)
            {
                // Detached (No tracking)
                query = query.AsNoTracking();
            }

            // Return IQueryable, which can be further processed
            return Task.FromResult(query);
        }
    }

}

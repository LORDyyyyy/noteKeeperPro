using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NoteKeeperPro.Domain.Entities.Collaborators;
using NoteKeeperPro.Domain.Entities.ApplicationUsers;
using NoteKeeperPro.Infrastructure.Presistance.Data;

namespace NoteKeeperPro.Infrastructure.Presistance.Repositories.Collaborators
{
    public class CollaboratorRepository : ICollaboratorRepository
    {
        private readonly ApplicationDbContext _context;

        public CollaboratorRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Collaborator> GetAll(bool AsNoTracking = true)
        {
            var query = _context.Collaborators
                .Include(c => c.User)
                .AsQueryable();
            if (AsNoTracking)
                query = query.AsNoTracking();
            return query.ToList();
        }

        public IQueryable<Collaborator> GetAllQuarable(bool AsNoTracking = true)
        {
            var query = _context.Collaborators
                .Include(c => c.User)
                .AsQueryable();
            if (AsNoTracking)
                query = query.AsNoTracking();
            return query;
        }

        public Collaborator GetById(int id)
        {
            return _context.Collaborators
                .Include(c => c.User)
                .FirstOrDefault(c => c.Id == id);
        }

        public int AddCollaborator(Collaborator collaborator)
        {
            _context.Collaborators.Add(collaborator);
            return _context.SaveChanges();
        }

        public int UpdateCollaborator(Collaborator collaborator)
        {
            _context.Collaborators.Update(collaborator);
            return _context.SaveChanges();
        }

        public int DeleteCollaborator(Collaborator collaborator)
        {
            _context.Collaborators.Remove(collaborator);
            return _context.SaveChanges();
        }

        public async Task<Collaborator> GetOrCreateCollaboratorAsync(string email)
        {
            var collaborator = await _context.Collaborators
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.User.Email == email);

            if (collaborator == null)
            {
                // Create a new user first
                var user = new ApplicationUser { Email = email };
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();

                // Then create the collaborator
                collaborator = new Collaborator { User = user };
                await _context.Collaborators.AddAsync(collaborator);
                await _context.SaveChangesAsync();
            }

            return collaborator;
        }

        public async Task<Collaborator> GetByEmailAsync(string email)
        {
            return await _context.Collaborators
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.User.Email == email);
        }

        public async Task<Collaborator> GetByIdAsync(int id)
        {
            return await _context.Collaborators
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}

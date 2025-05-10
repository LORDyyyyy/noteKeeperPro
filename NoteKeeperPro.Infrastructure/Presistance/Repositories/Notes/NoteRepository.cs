using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NoteKeeperPro.Domain.Entities.M_M_RelationShips;
using NoteKeeperPro.Domain.Entities.Notes;
using NoteKeeperPro.Domain.Entities.ApplicationUsers;
using NoteKeeperPro.Infrastructure.Presistance.Data;

namespace NoteKeeperPro.Infrastructure.Presistance.Repositories.Notes
{
    public class NoteRepository : INoteRepository
    {
        private readonly ApplicationDbContext _context;

        public NoteRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Note> GetAll(bool AsNoTracking = true)
        {
            var query = _context.Notes.AsQueryable();
            if (AsNoTracking)
                query = query.AsNoTracking();
            return query.ToList();
        }

        public IQueryable<Note> GetAllQuarable(bool AsNoTracking = true)
        {
            var query = _context.Notes.AsQueryable();
            if (AsNoTracking)
                query = query.AsNoTracking();
            return query;
        }

        public Note GetById(int id)
        {
            return _context.Notes.Find(id);
        }

        public int AddNote(Note note)
        {
            _context.Notes.Add(note);
            return _context.SaveChanges();
        }

        public int UpdateNote(Note note)
        {
            _context.Notes.Update(note);
            return _context.SaveChanges();
        }

        public int DeleteNote(Note note)
        {
            _context.Notes.Remove(note);
            return _context.SaveChanges();
        }

        public async Task<Note> GetAsync(int id)
        {
            return await _context.Notes
                .Include(n => n.NoteTags)
                    .ThenInclude(nt => nt.Tag)
                .Include(n => n.NoteCollaborators)
                    .ThenInclude(nc => nc.Collaborator)
                        .ThenInclude(c => c.User)
                .FirstOrDefaultAsync(n => n.Id == id);
        }

        public async Task<Note> GetAsync(int id, string userId)
        {
            return await _context.Notes
                .Include(n => n.NoteTags)
                    .ThenInclude(nt => nt.Tag)
                .Include(n => n.NoteCollaborators)
                    .ThenInclude(nc => nc.Collaborator)
                        .ThenInclude(c => c.User)
                .FirstOrDefaultAsync(n => n.Id == id && n.OwnerId == userId);
        }

        public async Task<IEnumerable<Note>> GetAllAsync(string userId)
        {
            return await _context.Notes
                .Include(n => n.NoteTags)
                    .ThenInclude(nt => nt.Tag)
                .Include(n => n.NoteCollaborators)
                    .ThenInclude(nc => nc.Collaborator)
                        .ThenInclude(c => c.User)
                .Where(n => n.OwnerId == userId || n.NoteCollaborators.Any(nc => nc.Collaborator.UserId == userId))
                .ToListAsync();
        }

        public async Task<Note> CreateAsync(Note note)
        {
            await _context.Notes.AddAsync(note);
            await _context.SaveChangesAsync();
            return note;
        }

        public async Task<Note> UpdateAsync(Note note)
        {
            _context.Notes.Update(note);
            await _context.SaveChangesAsync();
            return note;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var note = await GetAsync(id);
            if (note == null)
                return false;

            _context.Notes.Remove(note);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id, string userId)
        {
            var note = await GetAsync(id, userId);
            if (note == null)
                return false;

            _context.Notes.Remove(note);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ShareAsync(int noteId, string collaboratorEmail, string userId)
        {
            var note = await GetAsync(noteId, userId);
            if (note == null)
                return false;

            var collaborator = await _context.Collaborators
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.User.Email == collaboratorEmail);

            if (collaborator == null)
                return false;

            note.NoteCollaborators.Add(new NoteCollaborator { Collaborator = collaborator });
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveCollaboratorAsync(int noteId, string collaboratorEmail, string userId)
        {
            var note = await GetAsync(noteId, userId);
            if (note == null)
                return false;

            var collaborator = await _context.Collaborators
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.User.Email == collaboratorEmail);

            if (collaborator == null)
                return false;

            var noteCollaborator = note.NoteCollaborators
                .FirstOrDefault(nc => nc.Collaborator.Id == collaborator.Id);

            if (noteCollaborator == null)
                return false;

            note.NoteCollaborators.Remove(noteCollaborator);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task AddAsync(Note note)
        {
            await _context.Notes.AddAsync(note);
            await _context.SaveChangesAsync();
        }
    }
}

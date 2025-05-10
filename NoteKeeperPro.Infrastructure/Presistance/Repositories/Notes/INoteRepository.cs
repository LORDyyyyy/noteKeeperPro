using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoteKeeperPro.Domain.Entities.Notes;

namespace NoteKeeperPro.Infrastructure.Presistance.Repositories.Notes
{
    public interface INoteRepository
    {
        IEnumerable<Note> GetAll(bool AsNoTracking = true);
        IQueryable<Note> GetAllQuarable(bool AsNoTracking = true);
        Note GetById(int id);
        int AddNote(Note note);
        int UpdateNote(Note note);
        int DeleteNote(Note note);
        Task<Note> GetAsync(int id);
        Task<Note> GetAsync(int id, string userId);
        Task<IEnumerable<Note>> GetAllAsync(string userId);
        Task<Note> CreateAsync(Note note);
        Task<Note> UpdateAsync(Note note);
        Task<bool> DeleteAsync(int id);
        Task<bool> DeleteAsync(int id, string userId);
        Task<bool> ShareAsync(int noteId, string collaboratorEmail, string userId);
        Task<bool> RemoveCollaboratorAsync(int noteId, string collaboratorEmail, string userId);
        Task AddAsync(Note note);
    }
}

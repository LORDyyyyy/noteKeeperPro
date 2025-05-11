using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoteKeeperPro.Application.Dtos.Notes;

namespace NoteKeeperPro.Application.Services.Notes
{
    public interface INoteService
    {
        Task<NoteDetailsToReturnDto> GetNoteByIdAsync(int id, string userId);
        Task<IEnumerable<NoteToReturnDto>> GetAllNotesAsync(string userId);
        Task<IEnumerable<NoteToReturnDto>> SearchNotesAsync(string userId, string searchTerm);
        Task<NoteToReturnDto> CreateNoteAsync(CreateNoteDto createNoteDto, string userId);
        Task<NoteToReturnDto> UpdateNoteAsync(UpdateNoteDto updateNoteDto, string userId);
        Task<bool> DeleteNoteAsync(int id, string userId);
        Task<bool> ShareNoteAsync(int noteId, string collaboratorEmail, string userId);
        Task<bool> RemoveCollaboratorAsync(int noteId, string collaboratorEmail, string userId);

        // New methods for recycle bin functionality
        Task<IEnumerable<NoteToReturnDto>> GetDeletedNotesAsync(string userId);
        Task<bool> RestoreNoteAsync(int id, string userId);
        Task<bool> PermanentlyDeleteNoteAsync(int id, string userId);
    }
}

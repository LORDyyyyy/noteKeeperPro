using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NoteKeeperPro.Application.Dtos.Notes;
using NoteKeeperPro.Domain.Entities.Notes;
using NoteKeeperPro.Domain.Entities.Tags;
using NoteKeeperPro.Domain.Entities.M_M_RelationShips;
using NoteKeeperPro.Infrastructure.Presistance.Data;
using NoteKeeperPro.Infrastructure.Presistance.Repositories.Notes;
using NoteKeeperPro.Infrastructure.Presistance.Repositories.Tags;
using NoteKeeperPro.Infrastructure.Presistance.Repositories.Collaborators;
using NoteKeeperPro.Application.Dtos.Collaborators;
using NoteKeeperPro.Application.Dtos.Tags;
using NoteKeeperPro.Application.Dtos.ApplicationsUsers;
using NoteKeeperPro.Application.Dtos.NotesInfo;

namespace NoteKeeperPro.Application.Services.Notes
{
    public class NoteService : INoteService
    {
        private readonly INoteRepository _noteRepository;
        private readonly ITagRepository _tagRepository;
        private readonly ICollaboratorRepository _collaboratorRepository;
        private readonly ApplicationDbContext _context;

        public NoteService(
            INoteRepository noteRepository,
            ITagRepository tagRepository,
            ICollaboratorRepository collaboratorRepository,
            ApplicationDbContext context)
        {
            _noteRepository = noteRepository;
            _tagRepository = tagRepository;
            _collaboratorRepository = collaboratorRepository;
            _context = context;
        }

        public async Task<NoteDetailsToReturnDto> GetNoteByIdAsync(int id, string userId)
        {
            var note = await _context.Notes
                .Include(n => n.Owner)
                .Include(n => n.NoteInfo)
                .Include(n => n.NoteCollaborators)
                    .ThenInclude(nc => nc.Collaborator)
                        .ThenInclude(c => c.User)
                .Include(n => n.NoteTags)
                    .ThenInclude(nt => nt.Tag)
                .FirstOrDefaultAsync(n => n.Id == id);

            if (note == null || note.OwnerId != userId)
                throw new KeyNotFoundException("Note not found or access denied");

            return MapToDetailsReturnDto(note);
        }

        public async Task<IEnumerable<NoteToReturnDto>> GetAllNotesAsync(string userId)
        {
            var notes = await _noteRepository.GetAllAsync(userId);
            return notes
                .Where(n => n.OwnerId.Equals(userId) || n.NoteCollaborators.Any(c => c.Collaborator.Id.Equals(userId)))
                .Select(MapToReturnDto);
        }

        public async Task<IEnumerable<NoteToReturnDto>> SearchNotesAsync(string userId, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return await GetAllNotesAsync(userId);

            var notes = await _noteRepository.GetAllAsync(userId);
            return notes
                .Where(n => (n.OwnerId.Equals(userId) || n.NoteCollaborators.Any(c => c.Collaborator.Id.Equals(userId))) &&
                           (n.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                            n.Content.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                            n.NoteTags.Any(t => t.Tag.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))))
                .Select(MapToReturnDto);
        }

        public async Task<NoteToReturnDto> CreateNoteAsync(CreateNoteDto createNoteDto, string userId)
        {
            var note = new Note
            {
                Title = createNoteDto.Title,
                Content = createNoteDto.Content,
                OwnerId = userId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            // Handle tags
            foreach (var tagName in createNoteDto.TagNames)
            {
                var tag = await _tagRepository.GetOrCreateTagAsync(tagName);
                note.NoteTags.Add(new NoteTag { Tag = tag });
            }

            // Handle collaborators
            foreach (var email in createNoteDto.CollaboratorEmails)
            {
                var collaborator = await _collaboratorRepository.GetOrCreateCollaboratorAsync(email);
                note.NoteCollaborators.Add(new NoteCollaborator { Collaborator = collaborator });
            }

            await _noteRepository.AddAsync(note);
            await _context.SaveChangesAsync();

            return MapToReturnDto(note);
        }

        public async Task<NoteToReturnDto> UpdateNoteAsync(UpdateNoteDto updateNoteDto, string userId)
        {
            var note = await _context.Notes
                .Include(n => n.NoteTags)
                    .ThenInclude(nt => nt.Tag)
                .Include(n => n.NoteCollaborators)
                    .ThenInclude(nc => nc.Collaborator)
                .FirstOrDefaultAsync(n => n.Id == updateNoteDto.Id);

            if (note == null || note.OwnerId != userId)
                throw new KeyNotFoundException("Note not found or access denied");

            // Update basic properties
            note.Title = updateNoteDto.Title;
            note.Content = updateNoteDto.Content;
            note.UpdatedAt = DateTime.UtcNow;

            // Update tags - remove existing ones that are not in the new list
            var existingTagNames = note.NoteTags.Select(nt => nt.Tag.Name).ToList();
            var newTagNames = updateNoteDto.TagNames.ToList();

            // Remove tags that are no longer present
            var tagsToRemove = note.NoteTags
                .Where(nt => !newTagNames.Contains(nt.Tag.Name))
                .ToList();
            foreach (var tagToRemove in tagsToRemove)
            {
                note.NoteTags.Remove(tagToRemove);
            }

            // Add new tags
            foreach (var tagName in newTagNames.Where(tn => !existingTagNames.Contains(tn)))
            {
                var tag = await _tagRepository.GetOrCreateTagAsync(tagName);
                note.NoteTags.Add(new NoteTag { Tag = tag });
            }

            // Update collaborators - remove existing ones that are not in the new list
            var existingCollaboratorEmails = note.NoteCollaborators
                .Select(nc => nc.Collaborator.User.Email)
                .ToList();
            var newCollaboratorEmails = updateNoteDto.CollaboratorEmails.ToList();

            // Remove collaborators that are no longer present
            var collaboratorsToRemove = note.NoteCollaborators
                .Where(nc => !newCollaboratorEmails.Contains(nc.Collaborator.User.Email))
                .ToList();
            foreach (var collaboratorToRemove in collaboratorsToRemove)
            {
                note.NoteCollaborators.Remove(collaboratorToRemove);
            }

            // Add new collaborators
            foreach (var email in newCollaboratorEmails.Where(e => !existingCollaboratorEmails.Contains(e)))
            {
                var collaborator = await _collaboratorRepository.GetOrCreateCollaboratorAsync(email);
                note.NoteCollaborators.Add(new NoteCollaborator { Collaborator = collaborator });
            }

            await _context.SaveChangesAsync();
            return MapToReturnDto(note);
        }

        public async Task<bool> DeleteNoteAsync(int id, string userId)
        {
            var note = await _noteRepository.GetAsync(id);
            if (note == null || note.OwnerId != userId)
                return false;

            // Delete related entities first
            if (note.NoteInfo != null)
            {
                _context.NoteInfos.Remove(note.NoteInfo);
            }

            // Remove all note tags
            note.NoteTags.Clear();

            // Remove all note collaborators
            note.NoteCollaborators.Clear();

            // Save changes to remove related entities
            await _context.SaveChangesAsync();

            // Now delete the note itself
            return await _noteRepository.DeleteAsync(id, userId);
        }

        public async Task<bool> ShareNoteAsync(int noteId, string collaboratorEmail, string userId)
        {
            var note = await _noteRepository.GetAsync(noteId);
            if (note == null || note.OwnerId != userId)
                return false;

            var collaborator = await _collaboratorRepository.GetOrCreateCollaboratorAsync(collaboratorEmail);
            note.NoteCollaborators.Add(new NoteCollaborator { Collaborator = collaborator });

            await _noteRepository.UpdateAsync(note);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveCollaboratorAsync(int noteId, string collaboratorEmail, string userId)
        {
            var note = await _noteRepository.GetAsync(noteId);
            if (note == null || note.OwnerId != userId)
                return false;

            var collaborator = await _collaboratorRepository.GetByEmailAsync(collaboratorEmail);
            if (collaborator == null)
                return false;
            var noteCollaborator = note.NoteCollaborators
                .FirstOrDefault(nc => nc.Collaborator.Id == collaborator.Id);

            if (noteCollaborator != null)
            {
                note.NoteCollaborators.Remove(noteCollaborator);
                await _noteRepository.UpdateAsync(note);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        private static NoteToReturnDto MapToReturnDto(Note note)
        {
            return new NoteToReturnDto
            {
                Id = note.Id,
                Title = note.Title,
                Content = note.Content,
                Collaborators = note.NoteCollaborators.Select(nc => new CollaboratorDetailsToReturnDto
                {
                    Id = nc.Collaborator.Id,
                    NoteId = note.Id,
                    UserId = nc.Collaborator.UserId,
                    UserName = nc.Collaborator.User.UserName,
                    PermissionType = nc.Collaborator.PermissionType,
                    IsDeleted = nc.Collaborator.IsDeleted
                }).ToList(),
                Tags = note.NoteTags.Select(nt => new TagDetailsToReturnDto
                {
                    Id = nt.Tag.Id,
                    Name = nt.Tag.Name,
                    IsDeleted = nt.Tag.IsDeleted,
                    NoteIds = new List<int> { note.Id }
                }).ToList()
            };
        }

        private static NoteDetailsToReturnDto MapToDetailsReturnDto(Note note)
        {
            if (note == null)
                throw new ArgumentNullException(nameof(note));

            return new NoteDetailsToReturnDto
            {
                Id = note.Id,
                Title = note.Title,
                Content = note.Content,
                CreatedAt = note.CreatedAt,
                UpdatedAt = note.UpdatedAt,
                IsDeleted = note.IsDeleted,
                Owner = note.Owner != null ? new ApplicationUserDto
                {
                    Id = note.Owner.Id,
                    UserName = note.Owner.UserName,
                    Email = note.Owner.Email
                } : null,
                NoteInfo = note.NoteInfo != null ? new NoteInfoDetailsToReturnDto
                {
                    Id = note.NoteInfo.Id,
                    NoteId = note.Id,
                    CreatedAt = note.CreatedAt,
                    LastModifiedAt = note.UpdatedAt,
                    WordCount = note.Content.Split(new[] { ' ', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Length,
                    CharchterCount = note.Content.Length,
                    IsDeleted = note.NoteInfo.IsDeleted
                } : new NoteInfoDetailsToReturnDto
                {
                    Id = 0,
                    NoteId = note.Id,
                    CreatedAt = note.CreatedAt,
                    LastModifiedAt = note.UpdatedAt,
                    WordCount = note.Content.Split(new[] { ' ', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Length,
                    CharchterCount = note.Content.Length,
                    IsDeleted = false
                },
                Collaborators = note.NoteCollaborators?.Select(nc => new CollaboratorDetailsToReturnDto
                {
                    Id = nc.Collaborator.Id,
                    NoteId = note.Id,
                    UserId = nc.Collaborator.UserId,
                    UserName = nc.Collaborator.User.UserName,
                    PermissionType = nc.Collaborator.PermissionType,
                    IsDeleted = nc.Collaborator.IsDeleted
                }).ToList() ?? new List<CollaboratorDetailsToReturnDto>(),
                Tags = note.NoteTags?.Select(nt => new TagDetailsToReturnDto
                {
                    Id = nt.Tag.Id,
                    Name = nt.Tag.Name,
                    IsDeleted = nt.Tag.IsDeleted,
                    NoteIds = new List<int> { note.Id }
                }).ToList() ?? new List<TagDetailsToReturnDto>()
            };
        }
    }
}

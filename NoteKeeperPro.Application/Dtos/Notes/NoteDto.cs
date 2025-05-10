using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using NoteKeeperPro.Domain.Entities.Tags;
using NoteKeeperPro.Domain.Entities.Collaborators;

namespace NoteKeeperPro.Application.Dtos.Notes
{
    public class NoteDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string OwnerId { get; set; } = null!;
        public List<string> TagNames { get; set; } = new();
        public List<string> CollaboratorEmails { get; set; } = new();
        public List<Tag> Tags { get; set; } = new();
        public List<Collaborator> Collaborators { get; set; } = new();
    }

    public class CreateNoteDto
    {
        [Required(ErrorMessage = "Title is required")]
        [StringLength(200, ErrorMessage = "Title cannot be longer than 200 characters")]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "Content is required")]
        public string Content { get; set; } = null!;

        public string TagNamesString { get; set; } = string.Empty;
        public string CollaboratorEmailsString { get; set; } = string.Empty;

        public List<string> TagNames
        {
            get => string.IsNullOrEmpty(TagNamesString)
                ? new List<string>()
                : TagNamesString.Split(',').Select(t => t.Trim()).Where(t => !string.IsNullOrEmpty(t)).ToList();
            set => TagNamesString = string.Join(",", value);
        }

        public List<string> CollaboratorEmails
        {
            get => string.IsNullOrEmpty(CollaboratorEmailsString)
                ? new List<string>()
                : CollaboratorEmailsString.Split(',').Select(e => e.Trim()).Where(e => !string.IsNullOrEmpty(e)).ToList();
            set => CollaboratorEmailsString = string.Join(",", value);
        }
    }

    public class UpdateNoteDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(200, ErrorMessage = "Title cannot be longer than 200 characters")]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "Content is required")]
        public string Content { get; set; } = null!;

        public string TagNamesString { get; set; } = string.Empty;
        public string CollaboratorEmailsString { get; set; } = string.Empty;

        public List<string> TagNames
        {
            get => string.IsNullOrEmpty(TagNamesString)
                ? new List<string>()
                : TagNamesString.Split(',').Select(t => t.Trim()).Where(t => !string.IsNullOrEmpty(t)).ToList();
            set => TagNamesString = string.Join(",", value);
        }

        public List<string> CollaboratorEmails
        {
            get => string.IsNullOrEmpty(CollaboratorEmailsString)
                ? new List<string>()
                : CollaboratorEmailsString.Split(',').Select(e => e.Trim()).Where(e => !string.IsNullOrEmpty(e)).ToList();
            set => CollaboratorEmailsString = string.Join(",", value);
        }
    }
}
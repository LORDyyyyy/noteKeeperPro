using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoteKeeperPro.Domain.Entities.Notes;

namespace NoteKeeperPro.Domain.Entities.NotesInfo
{
    public class NoteInfo
    {
        public int Id { get; set; }

        // FK to the corresponding note
        public int NoteId { get; set; }

        // Navigation to the note
        public Note Note { get; set; } = null!;

        // Timestamp of creation
        public DateTime CreatedAt { get; set; }

        // Timestamp of last modification
        public DateTime LastModifiedAt { get; set; }

        // Count of words in the note
        public int WordCount { get; set; }

        // Count of characters in the note
        public int CharchterCount { get; set; }

        // Soft delete flag
        public bool IsDeleted { get; set; } = false;
    }

}

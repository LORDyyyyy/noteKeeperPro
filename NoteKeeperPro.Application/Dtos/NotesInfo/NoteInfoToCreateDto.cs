using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoteKeeperPro.Domain.Entities.Notes;

namespace NoteKeeperPro.Application.Dtos.NotesInfo
{
    public class NoteInfoToCreateDto
    {
        public DateTime CreatedAt { get; set; }
        public DateTime LastModifiedAt { get; set; }

        public Note Note { get; set; } = null!;
        public int WordCount { get; set; }
        public int CharchterCount { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}

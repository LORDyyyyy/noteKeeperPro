using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoteKeeperPro.Domain.Entities.Notes;

namespace NoteKeeperPro.Application.Dtos.NotesInfo
{
    public class NoteInfoDetailsToReturnDto
    {
        public int Id { get; set; }

        public int NoteId { get; set; }

        public Note Note { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public DateTime LastModifiedAt { get; set; }

        public int WordCount { get; set; }

        public int CharchterCount { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}

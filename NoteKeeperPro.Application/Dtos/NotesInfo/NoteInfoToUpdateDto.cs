using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteKeeperPro.Application.Dtos.NotesInfo
{
    public class NoteInfoToUpdateDto
    {
        public int Id { get; set; }

        public int NoteId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastModifiedAt { get; set; }
        public int WordCount { get; set; }
        public int CharchterCount { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}

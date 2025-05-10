using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoteKeeperPro.Domain.Entities.Notes;

namespace NoteKeeperPro.Application.Dtos.Tags
{
    public class TagDetailsToReturnDto
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public bool IsDeleted { get; set; } = false;

        public ICollection<int> NoteIds { get; set; } = new List<int>();
    }
}

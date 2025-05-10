using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoteKeeperPro.Domain.Entities.M_M_RelationShips;
using NoteKeeperPro.Domain.Entities.Notes;

namespace NoteKeeperPro.Domain.Entities.Tags
{
    public class Tag
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        // Soft delete flag
        public bool IsDeleted { get; set; } = false;


        // علاقة مع Note عبر NoteTag
        public ICollection<NoteTag> NoteTags { get; set; } = new HashSet<NoteTag>();

        // Notes associated with this tag (many-to-many)
        public List<int> NoteIds { get; set; } = new List<int>();
    }

}

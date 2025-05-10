using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoteKeeperPro.Domain.Entities.Notes;
using NoteKeeperPro.Domain.Entities.Tags;

namespace NoteKeeperPro.Domain.Entities.M_M_RelationShips
{
    public class NoteTag
    {
        public int NoteId { get; set; }
        public Note Note { get; set; }

        public int TagId { get; set; }
        public Tag Tag { get; set; }
    }

}

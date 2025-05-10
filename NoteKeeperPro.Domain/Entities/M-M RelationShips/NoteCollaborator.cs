using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoteKeeperPro.Domain.Entities.Collaborators;
using NoteKeeperPro.Domain.Entities.Notes;

namespace NoteKeeperPro.Domain.Entities.M_M_RelationShips
{
    public class NoteCollaborator
    {
        public int NoteId { get; set; }
        public Note Note { get; set; }

        public int CollaboratorId { get; set; }
        public Collaborator Collaborator { get; set; }
    }
}

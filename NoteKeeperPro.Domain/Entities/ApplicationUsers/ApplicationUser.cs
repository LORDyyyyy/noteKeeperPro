using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using NoteKeeperPro.Domain.Entities.Collaborators;
using NoteKeeperPro.Domain.Entities.Notes;

namespace NoteKeeperPro.Domain.Entities.ApplicationUsers
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsAgree { get; set; } = false;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public ICollection<Note> Notes { get; set; }
        public ICollection<Collaborator> Collaborators { get; set; } = new HashSet<Collaborator>();


    }
}

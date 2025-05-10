using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoteKeeperPro.Domain.Entities.ApplicationUsers;

namespace NoteKeeperPro.Application.Dtos.ApplicationsUsers
{
    public class ApplicationUserDto
    {
        public string Id { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public bool IsAgree { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

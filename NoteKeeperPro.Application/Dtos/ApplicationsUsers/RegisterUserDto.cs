using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoteKeeperPro.Domain.Entities.ApplicationUsers;


namespace NoteKeeperPro.Application.Dtos.ApplicationUsers
{
    public class RegisterUserDto
    {
        public string Email { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public bool IsAgree { get; set; }
    }

}

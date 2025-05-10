using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoteKeeperPro.Domain.Entities.ApplicationUsers;


namespace NoteKeeperPro.Application.Dtos.ApplicationsUsers
{
    public class LoginUserDto
    {
        public string UserNameOrEmail { get; set; } = null!;
        public string Password { get; set; } = null!;
    }

}

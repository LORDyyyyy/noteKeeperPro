using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoteKeeperPro.Domain.Entities.ApplicationUsers;


namespace NoteKeeperPro.Application.Dtos.ApplicationsUsers
{
    public class ConfirmEmailDto
    {
        public string UserId { get; set; } = null!;
        public string Token { get; set; } = null!;
    }

}

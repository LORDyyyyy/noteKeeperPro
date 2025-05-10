using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoteKeeperPro.Domain.Entities.ApplicationUsers;


namespace NoteKeeperPro.Application.Dtos.ApplicationUsers
{
    public class ChangePasswordDto
    {
        public string OldPassword { get; set; } = null!;
        public string NewPassword { get; set; } = null!;
        public string ConfirmNewPassword { get; set; } = null!;
    }

}

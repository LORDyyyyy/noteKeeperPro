using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoteKeeperPro.Infrastructure.Identity;

namespace NoteKeeperPro.Application.Common.Services.EmailSettings
{
    public interface IEmailSettings
    {
        public void SendEmail(Email email);
    }
}

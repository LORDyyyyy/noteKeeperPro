using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using NoteKeeperPro.Infrastructure.Identity;

namespace NoteKeeperPro.Application.Common.Services.EmailSettings
{
    public class EmailSettings : IEmailSettings
    {
        public void SendEmail(Email email)
        {
           var client = new SmtpClient("smt.gmail.com",587); // 587 => using Tls
            client.EnableSsl = true; // lazem enable ssl to encrypt Connection

            client.Credentials = new NetworkCredential("mt7993047@gmail.com", " Application Password"); //Sender data , Generate Password
            client.Send("mt7993047@gmail.com",email.To,email.subject,email.body);
        }
    }
}

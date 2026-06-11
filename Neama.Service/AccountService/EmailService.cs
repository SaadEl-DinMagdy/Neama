using Microsoft.Extensions.Configuration;
using Neama.Core.Services.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Neama.Service.AccountService
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configration;

        public EmailService(IConfiguration configration)
        {
            _configration = configration;
        }
        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {

            var SmtpClient = new SmtpClient(_configration["EmailSettings:Host"], int.Parse(_configration["EmailSettings:Port"]))
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_configration["EmailSettings:Email"] , _configration["EmailSettings:Password"])
            };

            var mailMessage = new MailMessage(_configration["EmailSettings:Email"], toEmail, subject, body);

            mailMessage.IsBodyHtml=true;

            await SmtpClient.SendMailAsync(mailMessage);


        }
    }
}

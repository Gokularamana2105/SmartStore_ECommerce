using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;


namespace SmartStoreModelsUtility
{
    
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _config;
        public EmailSender(IConfiguration config)
        {
            _config = config;
        }
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var smtpServer = _config["EmailSettings:SmtpServer"];
            var smtpPort = int.Parse(_config["EmailSettings:SmtpPort"]);
            var senderName = _config["EmailSettings:SenderName"];
            var senderEmail = _config["EmailSettings:SenderEmail"];
            var password = _config["EmailSettings:Password"];

            var client = new SmtpClient(smtpServer)
            {
                Port= smtpPort,
                Credentials=new NetworkCredential(senderEmail,password),
                EnableSsl=true,
            };

            var mailMessage = new MailMessage
            {
                From=new MailAddress(senderEmail,senderName),
                Subject=subject,
                Body=htmlMessage,
                IsBodyHtml=true,
            };

            mailMessage.To.Add(email);
            return client.SendMailAsync(mailMessage);
        }
    }
}

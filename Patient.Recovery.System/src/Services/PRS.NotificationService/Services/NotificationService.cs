using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace PRS.NotificationService.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IConfiguration _configuration;

        public NotificationService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task Send(string message, string recipientEmail)
        {
            var smtpClient = new SmtpClient
            {
                Host = _configuration["Email:Smtp:Host"]!,
                Port = int.Parse(_configuration["Email:Smtp:Port"]!),
                Credentials = new NetworkCredential(
                    _configuration["Email:Smtp:Username"],
                    _configuration["Email:Smtp:Password"]
                ),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_configuration["Email:From"]!),
                Subject = "Patient Monitoring Alert",
                Body = message,
                IsBodyHtml = false,
            };

            mailMessage.To.Add(recipientEmail);

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutomatedTaskSchedulingSystem.Utility
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var settings = _configuration.GetSection("EmailSettings");

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(
                settings["FromName"],
                settings["FromEmail"]
            ));

            message.To.Add(MailboxAddress.Parse(email));
            message.Subject = subject;

            message.Body = new TextPart("html")
            {
                Text = htmlMessage
            };

            using var client = new SmtpClient();

            await client.ConnectAsync(
                settings["Host"],
                int.Parse(settings["Port"]),
                SecureSocketOptions.StartTls
            );

            await client.AuthenticateAsync(
                settings["UserName"],
                settings["Password"]
            );

            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}

﻿using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using TaskManager.Core.DTOs;
using TaskManager.Core.Interfaces.Services;

namespace TaskManager.Infrastructure.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailConfigurationSettings _emailConfigurationSettings;
        private readonly ILogger<EmailSender> _logger;

        public EmailSender(
            IOptionsMonitor<EmailConfigurationSettings> optionsMonitor,
            ILogger<EmailSender> logger
            )
        {
            _emailConfigurationSettings = optionsMonitor.CurrentValue;
            _logger = logger;
        }

        public async Task SendEmailAsync(EmailMessageDto message, TextFormat textFormat = TextFormat.Text)
        {
            var mailMessage = CreateEmailMessage(message, textFormat);

            await SendAsync(mailMessage);
        }

        private async Task SendAsync(MimeMessage mailMessage)
        {
            using var client = new SmtpClient();
            try
            {
                await client.ConnectAsync(_emailConfigurationSettings.SmtpServer, _emailConfigurationSettings.Port, true);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                await client.AuthenticateAsync(_emailConfigurationSettings.UserName, _emailConfigurationSettings.Password);

                await client.SendAsync(mailMessage);
            }
            catch
            {
                //log an error message or throw an exception, or both.
                _logger.LogError("--> Send Email not success");
                throw;
            }
            finally
            {
                await client.DisconnectAsync(true);
                client.Dispose();
            }
        }

        private MimeMessage CreateEmailMessage(EmailMessageDto message, TextFormat textFormat)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("email", _emailConfigurationSettings.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            if (textFormat is TextFormat.Text)
            {
                emailMessage.Body = new TextPart(textFormat)
                {
                    Text = message.Content
                };
            }
            else
            {
                emailMessage.Body = new TextPart(textFormat)
                {
                    Text = message.Content
                };
            }

            return emailMessage;
        }
    }
}
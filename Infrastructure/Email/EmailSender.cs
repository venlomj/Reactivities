﻿using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Resend;

namespace Infrastructure.Email
{
    public class EmailSender(IResend resend, IConfiguration config) : IEmailSender<User>
    {
        public async Task SendConfirmationLinkAsync(User user, string email, string confirmationLink)
        {
            var subject = "Confirm your email address";
            var body = $@"
            <p>Hi {user.DisplayName}</p>
            <p>Please confirm your email by clicking the link below</p>
            <p><a href='{confirmationLink}'>Click here to verify email</a></p>
            <p>Thanks</p>
        ";

            await SendMailAsync(email, subject, body);
        }

        public Task SendPasswordResetLinkAsync(User user, string email, string resetLink)
        {
            throw new NotImplementedException();
        }

        public Task SendPasswordResetCodeAsync(User user, string email, string resetCode)
        {
            throw new NotImplementedException();
        }

        private async Task SendMailAsync(string email, string subject, string body)
        {
            var message = new EmailMessage
            {
                From = "reactivities-mj@resend.dev",
                Subject = subject,
                HtmlBody = body
            };
            message.To.Add(email);

            Console.WriteLine(message.HtmlBody);

            await resend.EmailSendAsync(message);

            // await Task.CompletedTask;
        }
    }
}

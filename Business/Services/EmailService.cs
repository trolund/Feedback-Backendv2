using System;
using System.Threading.Tasks;
using Business.Services.Interfaces;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Business.Services {
    public class EmailService : IEmailService {

        private string apiKey = null;

        public EmailService () {
            apiKey = Environment.GetEnvironmentVariable ("SENDGRID_API_KEY");
        }

        public Task SendEmailAsync (string email, string subject, string message) {
            return Execute (subject, message, email);
        }

        public Task Execute (string subject, string message, string email) {

            var client = new SendGridClient (apiKey);
            var msg = new SendGridMessage () {
                From = new EmailAddress ("Feedback@spinoff.dk"),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo (new EmailAddress (email));

            // Disable click tracking.
            // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            msg.SetClickTracking (false, false);

            return client.SendEmailAsync (msg);
        }
    }
}
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;

namespace BulkyBook.Utility
{
    public class EmailSender : IEmailSender
    {
        private readonly string _sendGridSecret;

        public EmailSender(IConfiguration config)
        {
            _sendGridSecret = config.GetValue<string>("SendGrid:SecretKey");
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            try
            {
                var client = new SendGridClient(_sendGridSecret);
                var from = new EmailAddress("mdvala0906@gmail.com", "Bulkynew");
                var to = new EmailAddress(email);
                var msg = MailHelper.CreateSingleEmail(from, to, subject, "", htmlMessage);
                var response = await client.SendEmailAsync(msg);

                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    Console.WriteLine($"Failed to send email. Status code: {response.StatusCode}");
                    var responseBody = await response.Body.ReadAsStringAsync();
                    Console.WriteLine($"Response body: {responseBody}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send email: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
}

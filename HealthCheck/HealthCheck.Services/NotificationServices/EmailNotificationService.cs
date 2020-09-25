using HealthCheck.Database;
using HealthCheck.Models;
using HealthCheck.Models.DTOs.TargetApps;
using HealthCheck.Models.DTOs.User;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HealthCheck.Services.NotificationServices
{

    public class EmailNotificationService : INotificationService
    {
        public void NotifyDown(UserDto user, TargetAppDto targetApp)
        {
            //send email
            Console.WriteLine($"Sending email to:{user.Name} - {user.Email} for down info of : {targetApp.Id} - {targetApp.Url}");

            using (SmtpClient smtpClient = new SmtpClient())
            {

                var message = new MimeMessage();
                message.To.Add(new MailboxAddress(user.Name, user.Email));
                message.From.Add(new MailboxAddress("Status Check Service", "checkhealth@yandex.com"));

                message.Body = new TextPart(TextFormat.Html)
                {
                    Text = $" App {targetApp.Name} is down Id : {targetApp.Id} - url {targetApp.Url}"
                };

                smtpClient.Connect("smtp.yandex.com", 465, true);
                smtpClient.Authenticate("checkhealth@yandex.com", "Hlt135!");
                
                //message is get rejected because of spam policy, I just wanted to show the implementation
                //smtpClient.Send(message);

                smtpClient.Disconnect(true);
            }
        }

    }

    public class EmailMessage
    {
        public EmailMessage()
        {
            ToAddresses = new List<EmailAddress>();
            FromAddresses = new List<EmailAddress>();
        }

        public List<EmailAddress> ToAddresses { get; set; }
        public List<EmailAddress> FromAddresses { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
    }
    public class EmailAddress
    {
        public string Name { get; set; }
        public string Address { get; set; }
    }
}

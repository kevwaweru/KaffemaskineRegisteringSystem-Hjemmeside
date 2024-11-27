using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Models.Enums;
using System.Net;
using System.Net.Mail;

namespace CoffeeCrazy.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="emailToResive"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public async Task<bool> SendEmailAsync(string emailToResive, string subject, string body)
        {
            try
            {
                string smtpSetting = SmtpSettings.SmtpSettings.ToString();
                string host = _configuration[$"{smtpSetting}:{SmtpSettings.Host}"];
                int.TryParse(_configuration[$"{smtpSetting}:{SmtpSettings.Port}"], out int port);
                string username = _configuration[$"{smtpSetting}:{SmtpSettings.Username}"];
                string password = _configuration[$"{smtpSetting}:{SmtpSettings.Password}"];

                var smtpClient = new SmtpClient(host)
                {
                    Port = port,
                    Credentials = new NetworkCredential(username, password),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(username, "Support"),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                };

                mailMessage.To.Add(emailToResive);

                await smtpClient.SendMailAsync(mailMessage);

                return true;
            }
            catch (Exception)
            {
               return false;
            }
        }
    }
}

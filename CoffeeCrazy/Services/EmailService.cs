using CoffeeCrazy.Interfaces;
using CoffeeCrazy.Models.Enums;
using CoffeeCrazy.Repos;
using System.Net;
using System.Net.Mail;

namespace CoffeeCrazy.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ITokenRepo _tokenGeneratorRepo;

        public EmailService(IConfiguration configuration, ITokenRepo tokenGeneratorRepo)
        {
            _configuration = configuration;
            _tokenGeneratorRepo = tokenGeneratorRepo;
        }

        /// <summary>
        /// Sendt an email with a body
        /// </summary>
        /// <param name="emailToResive">Resiveing email</param>
        /// <param name="subject">What it is about</param>
        /// <param name="body">The text</param>
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
        /// <summary>
        /// Generate at token and sends a email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<bool> GenerateTokenAndSendResetEmail(string email)
        {
            try
            {
                if () 
                await _tokenGeneratorRepo.CreateTokenAsync(email);


                string token = await _tokenGeneratorRepo.GetTokenAsync(email);

                if (token == null)
                {
                    throw new Exception("Failed to generate token");        
                }

                string subject = "Nyt password?";
                string body = $@"
                                <p>Din kode:{token}</p>
                                <p>Koden er gyldig i 30 min.</p>
                                <br/>
                                <p>Hvis du ikke har ansøgt om ny mail</p>
                                <p>kontakt JB på 112 </p>
                                <p>Mvh.</p>
                                <p>Support.</p>";


                return await SendEmailAsync(email, subject, body);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"error: {ex.Message}");
                return false;
            }
        }
    }
}

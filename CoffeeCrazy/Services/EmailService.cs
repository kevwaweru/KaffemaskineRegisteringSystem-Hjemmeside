using CoffeeCrazy.Interfaces;

namespace CoffeeCrazy.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {
                string smtpSetting = SmtpSettings.
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}

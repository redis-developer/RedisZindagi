using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using MimeKit;
using MimeKit.Text;
using Zindagi.Infra.Options;
using Zindagi.SeedWork;

namespace Zindagi.Infra.App
{
    public class Messaging : IMessaging
    {
        private readonly ILogger<Messaging> _logger;
        private readonly SmsOptions _smsOptions;
        private readonly SmtpOptions _smtpOptions;

        public Messaging(SmsOptions smsOptions, SmtpOptions smtpOptions, ILogger<Messaging> logger)
        {
            _smsOptions = smsOptions;
            _smtpOptions = smtpOptions;
            _logger = logger;
        }

        public async Task<bool> SendEmail(List<MailboxAddress> emailAddresses, string subject, string htmlMessage)
        {
            if (_smtpOptions.Disable)
                return false;

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Zindagi App", _smtpOptions.From));
            message.To.AddRange(emailAddresses);
            message.Subject = subject;

            message.Body = new TextPart(TextFormat.Html)
            {
                Text = @$"Dear {emailAddresses.FirstOrDefault()?.Name ?? "User"},
<br/><br/>
{htmlMessage}
<br/><br/>
Regards,
<b>Zindagi Team</b>"
            };

            using var client = new SmtpClient();
            await client.ConnectAsync(_smtpOptions.Host, _smtpOptions.Port, _smtpOptions.UseSsl);

            if (_smtpOptions.UserName.IsNotNullOrWhiteSpace() && _smtpOptions.Password.IsNotNullOrWhiteSpace())
                await client.AuthenticateAsync(_smtpOptions.UserName, _smtpOptions.Password);

            await client.SendAsync(message);
            await client.DisconnectAsync(true);
            return true;
        }

        public Task<bool> SendText(string mobileNumber, string message)
        {
            if (_smsOptions.Disable)
                return Task.FromResult(false);

            _logger.LogInformation("[SMS] sending text message. {smsOptions}", _smsOptions);
            // Task: Implement SMS API via Twilio and Msg91
            return Task.FromResult(true);
        }
    }
}

using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;

namespace Website.Services
{

    public sealed class SmtpOptions
    {
        public string Host { get; set; } = default!;
        public int Port { get; set; } = 587;
        public bool EnableSsl { get; set; } = true;
        public string UserName { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string From { get; set; } = default!;
    }

    public sealed class SmtpEmailSender : IEmailSender
    {
        private readonly SmtpOptions _opt;
        public SmtpEmailSender(IOptions<SmtpOptions> options) => _opt = options.Value;

        public async Task SendAsync(string to, string subject, string body, CancellationToken ct = default)
        {
            using var client = new SmtpClient(_opt.Host, _opt.Port)
            {
                EnableSsl = _opt.EnableSsl,
                Credentials = new NetworkCredential(_opt.UserName, _opt.Password)
            };

            using var msg = new MailMessage(_opt.From, to, subject, body)
            {
                BodyEncoding = System.Text.Encoding.UTF8,
                SubjectEncoding = System.Text.Encoding.UTF8
            };

            await client.SendMailAsync(msg);
        }
    }
}

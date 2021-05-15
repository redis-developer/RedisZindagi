namespace Zindagi.Infra.Options
{
    public class SmtpOptions
    {
        public bool Disable { get; set; }
        public string Host { get; set; } = string.Empty;
        public int Port { get; set; } = 25;
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string From { get; set; } = string.Empty;

        public bool UseSsl { get; set; }
    }
}

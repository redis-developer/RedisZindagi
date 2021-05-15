using System.Collections.Generic;
using MediatR;
using MimeKit;

namespace Zindagi.Domain.Common.Notifications
{
    public class SendEmailNotification : INotification
    {
        public SendEmailNotification(List<MailboxAddress> to, string subject, string htmlContent)
        {
            To = to;
            Subject = subject;
            HtmlContent = htmlContent;
        }

        public List<MailboxAddress> To { get; set; }
        public string Subject { get; set; }
        public string HtmlContent { get; set; }
    }
}

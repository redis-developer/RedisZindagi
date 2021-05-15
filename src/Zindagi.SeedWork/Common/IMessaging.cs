using System.Collections.Generic;
using System.Threading.Tasks;
using MimeKit;

namespace Zindagi.SeedWork
{
    public interface IMessaging
    {
        Task<bool> SendEmail(List<MailboxAddress> emailAddresses, string subject, string htmlMessage);
        Task<bool> SendText(string mobileNumber, string message);
    }
}

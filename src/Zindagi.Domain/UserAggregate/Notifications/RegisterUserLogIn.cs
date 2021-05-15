using System.Collections.Generic;
using System.Security.Claims;
using MediatR;

namespace Zindagi.Domain.UserAggregate.Notifications
{
    public class RegisterUserLogIn : INotification
    {
        public RegisterUserLogIn(List<Claim> claims) => Claims = claims;

        public List<Claim> Claims { get; set; }
    }
}

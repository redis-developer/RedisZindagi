using System;
using Zindagi.Domain.UserAggregate.Commands;
using Zindagi.SeedWork;

#nullable disable

namespace Zindagi.Domain.UserAggregate
{
    public class User : AggregateBase
    {
        public OpenIdKey AlternateId { get; set; }

        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }

        public string FullName => $"{FirstName} {MiddleName} {LastName}".Trim();

        public string MobileNumber { get; set; } = "";

        public string AlternateMobileNumber { get; set; }

        public string Email { get; set; }
        public Status IsVerified { get; set; }
        public Status IsActive { get; set; }
        public Status IsDonor { get; set; }
        public DateTime DateOfBirth { get; set; }

        public BloodGroup BloodGroup { get; set; }

        public int Age
        {
            get
            {
                var age = DateTime.Now.Year - DateOfBirth.Year;
                if (DateTime.Now.DayOfYear < DateOfBirth.DayOfYear)
                    age -= 1;

                return age;
            }
        }

        public static User Create(OpenIdUser openIdUser)
        {
            var result = new User
            {
                Id = Guid.NewGuid(),
                AlternateId = openIdUser.Id,
                Email = openIdUser.EmailAddress,
                IsVerified = openIdUser.IsEmailVerified ? Status.Active : Status.Inactive,
                FirstName = string.Empty,
                MiddleName = string.Empty,
                LastName = string.Empty,
                BloodGroup = BloodGroup.None
            };

            return result;
        }

        public void Update(UpdateUserInfo userInfo)
        {
            if (userInfo.FirstName.IsNotNullOrWhiteSpace())
                FirstName = userInfo.FirstName;

            if (userInfo.MiddleName.IsNotNullOrWhiteSpace())
                MiddleName = userInfo.MiddleName;

            if (userInfo.LastName.IsNotNullOrWhiteSpace())
                LastName = userInfo.LastName;

            if (userInfo.Email.IsNotNullOrWhiteSpace())
                Email = userInfo.Email;

            if (userInfo.MobileNumber.IsNotNullOrWhiteSpace())
                MobileNumber = userInfo.MobileNumber;

            BloodGroup = userInfo.BloodGroup;
            DateOfBirth = userInfo.DateOfBirth;
        }

        public override string GetPersistenceKey() => AlternateId.GetPersistenceKey();
    }
}

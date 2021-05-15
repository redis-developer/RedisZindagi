using System.ComponentModel;
using Zindagi.SeedWork;

namespace Zindagi.Domain
{
    public sealed class BloodGroup : Enumeration
    {
        public static readonly BloodGroup None = new(0, "-SELECT-");

        public static readonly BloodGroup APositive = new(1, "A +VE");
        public static readonly BloodGroup ANegative = new(2, "A -VE");

        public static readonly BloodGroup BPositive = new(3, "B +VE");
        public static readonly BloodGroup BNegative = new(4, "B -VE");

        public static readonly BloodGroup OPositive = new(5, "O +VE");
        public static readonly BloodGroup ONegative = new(6, "O -VE");

        public static readonly BloodGroup AbPositive = new(7, "AB +VE");
        public static readonly BloodGroup AbNegative = new(8, "AB -VE");

        private BloodGroup(int id, string name) : base(id, name) { }
    }

    public enum BloodGroupList
    {
        [Description("-SELECT-")]
        None = 0,

        [Description("A +Ve")]
        APositive = 1,

        [Description("A -VE")]
        ANegative = 2,

        [Description("B +Ve")]
        BPositive = 3,

        [Description("B -VE")]
        BNegative = 4,

        [Description("O +Ve")]
        OPositive = 5,

        [Description("O -VE")]
        ONegative = 6,

        [Description("AB +Ve")]
        AbPositive = 7,

        [Description("AB -VE")]
        AbNegative = 8
    }

    public sealed class BloodDonationType : Enumeration
    {
        public static readonly BloodDonationType None = new(0, "-SELECT-");

        public static readonly BloodDonationType WholeBloodDonation = new(1, "Whole Blood Donation");
        public static readonly BloodDonationType PlasmaDonation = new(2, "Plasma Donation (Apheresis)");
        public static readonly BloodDonationType PlateletDonation = new(3, "Platelet Donation (Plateletpheresis)");

        private BloodDonationType(int id, string name) : base(id, name) { }
    }

    public enum BloodDonationTypeList
    {
        [Description("-SELECT-")]
        None = 0,

        [Description("Whole Blood Donation")]
        WholeBloodDonation = 1,

        [Description("Plasma Donation (Apheresis)")]
        PlasmaDonation = 2,

        [Description("Platelet Donation (Plateletpheresis)")]
        PlateletDonation = 3
    }

    public sealed class Country : Enumeration
    {
        public static readonly Country None = new(0, "-SELECT-");

        public static readonly Country India = new(1, "INDIA");
        public static readonly Country UnitedStates = new(2, "UNITED STATES");
        public static readonly Country UnitedKingdom = new(3, "UNITED KINGDOM");

        private Country(int id, string name) : base(id, name) { }
    }

    public sealed class Status : Enumeration
    {
        public static readonly Status None = new(0, "-SELECT-");

        public static readonly Status Active = new(1, "ACTIVE");
        public static readonly Status Inactive = new(2, "IN ACTIVE");

        public Status(int id, string name) : base(id, name) { }
    }

    public enum StatusList
    {
        [Description("-SELECT-")]
        None = 0,

        [Description("Active")]
        Active = 1,

        [Description("In Active")]
        Inactive = 2
    }

    public sealed class DetailedStatus : Enumeration
    {
        public static readonly DetailedStatus None = new(0, "-SELECT-");
        public static readonly DetailedStatus Open = new(1, "OPEN");
        public static readonly DetailedStatus Assigned = new(2, "ASSIGNED");
        public static readonly DetailedStatus Cancelled = new(3, "CANCELLED");
        public static readonly DetailedStatus Duplicate = new(4, "DUPLICATE");
        public static readonly DetailedStatus Rejected = new(5, "REJECTED");
        public static readonly DetailedStatus Fulfilled = new(6, "FULFILLED");

        private DetailedStatus(int id, string name) : base(id, name) { }
    }

    public enum DetailedStatusList
    {
        [Description("-SELECT-")]
        None = 0,
        Open = 1,
        Assigned = 2,
        Cancelled = 3,
        Duplicate = 4,
        Rejected = 5,
        Fulfilled = 6
    }
}

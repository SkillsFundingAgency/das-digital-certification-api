namespace SFA.DAS.DigitalCertificates.Domain.Models
{
    public static class Enums
    {
        public enum ActionType
        {
            Reprint = 1,
            Help = 2,
            Contact = 3,
            NotMatched = 4,
            NotFound = 5,
        }
        public enum CertificateType
        {
            Unknown,
            Standard,
            Framework
        }
        public enum SharingStatus
        {
            Live = 0,
            Deleted = 1,
            Expired = 2
        }
        public enum UserActionStatus
        {
            New,
            Viewed
        }
        public enum AdminActionType
        {
            Viewed,
            Unlocked
        }
    }
}

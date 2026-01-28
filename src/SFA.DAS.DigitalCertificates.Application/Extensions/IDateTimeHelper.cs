using System;

namespace SFA.DAS.DigitalCertificates.Application.Extensions
{
    public interface IDateTimeHelper
    {
        DateTime Now { get; }
    }

    public class UtcTimeProvider : IDateTimeHelper
    {
        public DateTime Now => DateTime.UtcNow;
    }
}
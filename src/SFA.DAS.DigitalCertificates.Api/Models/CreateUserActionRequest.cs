using System;
using static SFA.DAS.DigitalCertificates.Domain.Models.Enums;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateUserAction;

namespace SFA.DAS.DigitalCertificates.Api.Models
{
    public class CreateUserActionRequest
    {
        public required ActionType ActionType { get; set; }
        public required string FamilyName { get; set; }
        public required string GivenNames { get; set; }
        public Guid? CertificateId { get; set; }
        public CertificateType? CertificateType { get; set; }
        public string? CourseName { get; set; }

        public static implicit operator CreateUserActionCommand(CreateUserActionRequest source)
        {
            return new CreateUserActionCommand
            {
                ActionType = source.ActionType,
                FamilyName = source.FamilyName,
                GivenNames = source.GivenNames,
                CertificateId = source.CertificateId,
                CertificateType = source.CertificateType,
                CourseName = source.CourseName
            };
        }
    }
}

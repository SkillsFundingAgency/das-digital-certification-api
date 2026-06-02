using MediatR;
using System;
using static SFA.DAS.DigitalCertificates.Domain.Models.Enums;

namespace SFA.DAS.DigitalCertificates.Application.Commands.CreateUserAction
{
    public class CreateUserActionCommand : IRequest<CreateUserActionCommandResponse>
    {
        public Guid UserId { get; set; }
        public required ActionType ActionType { get; set; }
        public required string FamilyName { get; set; }
        public required string GivenNames { get; set; }
        public Guid? CertificateId { get; set; }
        public CertificateType? CertificateType { get; set; }
        public string? CourseName { get; set; }
    }
}

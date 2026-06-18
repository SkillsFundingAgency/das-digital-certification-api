using System;
using MediatR;

namespace SFA.DAS.DigitalCertificates.Application.Queries.GetUserById
{
    public class GetUserByIdQuery : IRequest<GetUserByIdQueryResult>
    {
        public Guid UserId { get; set; }
    }
}

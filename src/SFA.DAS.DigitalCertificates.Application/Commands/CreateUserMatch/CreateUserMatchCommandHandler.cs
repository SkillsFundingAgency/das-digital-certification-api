using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.DigitalCertificates.Domain.Interfaces;
using SFA.DAS.DigitalCertificates.Domain.Entities;

namespace SFA.DAS.DigitalCertificates.Application.Commands.CreateUserMatch
{
    public class CreateUserMatchCommandHandler : IRequestHandler<CreateUserMatchCommand, Unit>
    {
        private readonly IUserMatchEntityContext _matchContext;
        private readonly IUserEntityContext _userContext;

        public CreateUserMatchCommandHandler(IUserMatchEntityContext matchContext, IUserEntityContext userContext)
        {
            _matchContext = matchContext;
            _userContext = userContext;
        }

        public async Task<Unit> Handle(CreateUserMatchCommand request, CancellationToken cancellationToken)
        {
            var user = await _userContext.GetByUserId(request.UserId);
            if (user == null)
            {
                return Unit.Value;
            }

            var entity = new UserMatch
            {
                UserId = request.UserId,
                Uln = request.Uln,
                FamilyName = request.FamilyName,
                DateOfBirth = request.DateOfBirth,
                CertificateType = request.CertificateType,
                CourseCode = request.CourseCode,
                CourseName = request.CourseName,
                CourseLevel = request.CourseLevel,
                DateAwarded = request.DateAwarded,
                ProviderName = request.ProviderName,
                Ukprn = request.Ukprn,
                IsMatched = request.IsMatched,
                IsFailed = request.IsFailed
            };

            _matchContext.Add(entity);

            if (request.IsFailed && !user.IsLocked)
            {
                user.IsLocked = true;
            }

            await _userContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}

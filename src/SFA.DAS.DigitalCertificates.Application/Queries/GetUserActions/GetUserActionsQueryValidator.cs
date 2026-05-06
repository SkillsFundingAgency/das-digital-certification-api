using FluentValidation;

namespace SFA.DAS.DigitalCertificates.Application.Queries.GetUserActions
{
    public class GetUserActionsQueryValidator : AbstractValidator<GetUserActionsQuery>
    {
        public GetUserActionsQueryValidator()
        {
            RuleFor(q => q.UserId).NotEmpty();
        }
    }
}

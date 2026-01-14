using FluentValidation;

namespace SFA.DAS.DigitalCertificates.Application.Queries.GetSharingById
{
    public class GetSharingByIdQueryValidator : AbstractValidator<GetSharingByIdQuery>
    {
        public GetSharingByIdQueryValidator()
        {
            RuleFor(x => x.SharingId)
                .NotEmpty()
                     .WithMessage("SharingId cannot be empty");

            RuleFor(x => x.Limit)
               .GreaterThan(0)
                   .When(x => x.Limit.HasValue)
              .WithMessage("Limit must be greater than zero when specified");
        }
    }
}
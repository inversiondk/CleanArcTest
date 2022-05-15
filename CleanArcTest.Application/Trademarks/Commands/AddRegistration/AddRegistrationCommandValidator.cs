using FluentValidation;

namespace CleanArcTest.Application.Trademarks.Commands.AddRegistration
{
    public class AddRegistrationCommandValidator : AbstractValidator<AddRegistrationCommand>
    {
        public AddRegistrationCommandValidator()
        {
            RuleFor(x => x.RenewalPrice).GreaterThan(0);
            RuleFor(x => x.CountryIso).NotEmpty().MaximumLength(2);
        }
    }
}

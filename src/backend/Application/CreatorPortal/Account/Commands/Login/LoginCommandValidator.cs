using FluentValidation;

namespace Application.CreatorPortal.Account.Commands.Login
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(v => v.Username)
                .NotNull()
                .NotEmpty();

            RuleFor(v => v.Password)
                .NotNull()
                .NotEmpty();
        }
    }
}

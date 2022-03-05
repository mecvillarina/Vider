using Application.Common.Constants;
using FluentValidation;

namespace Application.CreatorPortal.Account.Commands.Register
{
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            CascadeMode = CascadeMode.Stop;

            RuleFor(v => v.Username)
                .NotNull().WithMessage("Username is required.")
                .NotEmpty().WithMessage("Username is required.")
                .Matches(@"\A\S{3,20}\z").WithMessage("Username must be between 3 and 20 characters long and with no spaces.");

            RuleFor(v => v.Name)
                .NotNull().WithMessage("Name is required.")
                .NotEmpty().WithMessage("Name is required.")
                .MinimumLength(3).WithMessage("Name must be between 3 and 100 characters long.")
                .MaximumLength(100).WithMessage("Name must be between 3 and 100 characters long.");

            RuleFor(v => v.Bio)
                .NotNull().WithMessage("Bio is required.")
                .MaximumLength(140).WithMessage("Bio must be less than or equal to 140 characters long.");

            RuleFor(v => v.Password)
                .NotNull().WithMessage("Password is required.")
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(AppConstants.MinimumPasswordLength).WithMessage($"Password must be at least {AppConstants.MinimumPasswordLength} characters long.");

            RuleFor(v => v.ConfirmPassword)
                .NotNull().WithMessage("Confirm Password is required.")
                .NotEmpty().WithMessage("Confirm Password is required.")
                .Equal(v => v.Password).WithMessage("Password and Confirm Password must be the same.");
        }
    }
}

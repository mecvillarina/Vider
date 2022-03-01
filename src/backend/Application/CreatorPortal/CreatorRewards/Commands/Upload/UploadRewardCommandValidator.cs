using FluentValidation;

namespace Application.CreatorPortal.CreatorRewards.Commands.Upload
{
    public class UploadRewardCommandValidator : AbstractValidator<UploadRewardCommand>
    {
        public UploadRewardCommandValidator()
        {
            RuleFor(v => v.Name)
                .NotNull()
                .NotEmpty()
                .MinimumLength(3).WithMessage("Name must be between 3 and 100 characters long.")
                .MaximumLength(100).WithMessage("Name must be between 3 and 100 characters long."); 

            RuleFor(v => v.Message)
               .NotNull()
               .NotEmpty();
        }
    }
}

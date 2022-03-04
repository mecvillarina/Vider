using FluentValidation;

namespace Application.CreatorPortal.Feeds.Commands.CreatePost
{
    public class CreateFeedPostCommandValidator : AbstractValidator<CreateFeedPostCommand>
    {
        public CreateFeedPostCommandValidator()
        {
            RuleFor(v => v.Caption)
             .NotNull()
             //.NotEmpty()
             //.MinimumLength(5).WithMessage("Caption must be between 5 and 140 characters long.")
             .MaximumLength(140).WithMessage("Caption must be between 5 and 140 characters long.");
        }
    }
}

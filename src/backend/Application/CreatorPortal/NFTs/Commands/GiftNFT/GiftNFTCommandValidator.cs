using FluentValidation;

namespace Application.CreatorPortal.NFTs.Commands.GiftNFT
{
    public class GiftNFTCommandValidator : AbstractValidator<GiftNFTCommand>
    {
        public GiftNFTCommandValidator()
        {
            RuleFor(v => v.ReceiverUsername)
                    .NotNull().WithMessage("Username is required.")
                    .NotEmpty().WithMessage("Username is required.")
                    .Matches(@"\A\S{3,20}\z").WithMessage("Username must be between 3 and 20 characters long and with no spaces.");

            RuleFor(v => v.Message)
               .NotNull()
               .NotEmpty();
        }
    }
}

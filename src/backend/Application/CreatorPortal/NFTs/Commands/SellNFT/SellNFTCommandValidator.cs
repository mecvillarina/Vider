using FluentValidation;

namespace Application.CreatorPortal.NFTs.Commands.SellNFT
{
    public class SellNFTCommandValidator : AbstractValidator<SellNFTCommand>
    {
        public SellNFTCommandValidator()
        {
            RuleFor(v => v.Amount)
                .GreaterThan(0).WithMessage("For Testnet, NFT Sell Amount must be between NFT must be between 1 to 100 XRP")
                .LessThanOrEqualTo(100).WithMessage("For Testnet, NFT Sell Amount must be between NFT must be between 1 to 100 XRP");
        }
    }
}

using Application.Common.Mappings;
using Domain.Entities;

namespace Application.CreatorPortal.Account.Queries.GetWallet
{
    public class GetWalletResponse : IMapFrom<Creator>
    {
        public string AccountXAddress { get; set; }
        public string AccountClassicAddress { get; set; }
        public string AccountAddress { get; set; }
        public bool IsAccountValid { get; set; }
        public double Balance { get; set; }
    }
}

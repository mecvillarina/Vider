using Application.Common.Dtos.Response;
using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CreatorPortal.NFTs.Queries.GetTx
{
    public class GetTxQuery : IRequest<Result<string>>
    {
        public string TxHash { get; set; }

        public class GetTxQueryHandler : IRequestHandler<GetTxQuery, Result<string>>
        {
            private readonly IXrplNFTTokenService _tokenService;

            public GetTxQueryHandler(IXrplNFTTokenService tokenService)
            {
                _tokenService = tokenService;
            }

            public async Task<Result<string>> Handle(GetTxQuery request, CancellationToken cancellationToken)
            {
                var tx = _tokenService.GetTx(request.TxHash);
                var serialTx = JsonConvert.SerializeObject(tx, Formatting.Indented);
                return await Result<string>.SuccessAsync(data: serialTx);
            }
        }
    }
}

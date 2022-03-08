using Application.Common.Interfaces;
using Application.Common.Models;
using Application.CreatorPortal.Activities.Dtos;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CreatorPortal.Activities.Queries.GetRecent
{
    public class GetRecentActivityLogsQuery : IRequest<Result<List<ActivityLogDto>>>
    {
        public class GetRecentActivityLogsQueryHandler : IRequestHandler<GetRecentActivityLogsQuery, Result<List<ActivityLogDto>>>
        {
            private readonly ICallContext _context;
            private readonly IApplicationDbContext _dbContext;
            private readonly IMapper _mapper;

            public GetRecentActivityLogsQueryHandler(ICallContext context, IApplicationDbContext dbContext, IMapper mapper)
            {
                _context = context;
                _dbContext = dbContext;
                _mapper = mapper;
            }

            public async Task<Result<List<ActivityLogDto>>> Handle(GetRecentActivityLogsQuery request, CancellationToken cancellationToken)
            {
                var logs = _dbContext.ActivityLogs.AsQueryable().Where(x => x.CreatorId == _context.UserId && x.WalletAddress == _context.UserAccountAddress)
                    .OrderByDescending(x => x.DateOccured)
                    .Take(100)
                    .ProjectTo<ActivityLogDto>(_mapper.ConfigurationProvider)
                    .ToList();

                return await Result<List<ActivityLogDto>>.SuccessAsync(logs);
            }
        }
    }
}

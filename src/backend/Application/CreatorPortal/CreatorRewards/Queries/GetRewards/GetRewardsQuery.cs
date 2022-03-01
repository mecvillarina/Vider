using Application.Common.Constants;
using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CreatorPortal.CreatorRewards.Queries.GetRewards
{
    public class GetRewardsQuery : IRequest<Result<List<GetRewardItemDto>>>
    {
        public int CreatorId { get; set; }
        public class GetRewardsQueryHandler : IRequestHandler<GetRewardsQuery, Result<List<GetRewardItemDto>>>
        {
            private readonly ICallContext _context;
            private readonly IMapper _mapper;
            private readonly IApplicationDbContext _dbContext;
            private readonly IAzureStorageBlobService _blobService;
            private readonly IConfiguration _configuration;

            public GetRewardsQueryHandler(IMapper mapper, IApplicationDbContext dbContext, IAzureStorageBlobService blobService, ICallContext context, IConfiguration configuration)
            {
                _mapper = mapper;
                _dbContext = dbContext;
                _blobService = blobService;
                _context = context;
                _configuration = configuration;
            }

            public async Task<Result<List<GetRewardItemDto>>> Handle(GetRewardsQuery request, CancellationToken cancellationToken)
            {
                if (request.CreatorId == 0)
                    request.CreatorId = _context.UserId;

                var rewards = await _dbContext.CreatorRewards.AsQueryable().Where(x => x.CreatorId == request.CreatorId).ToListAsync();

                var mappedRewards = new List<GetRewardItemDto>();

                foreach (var reward in rewards)
                {
                    var mappedReward = _mapper.Map<GetRewardItemDto>(reward);
                    mappedReward.UrlLink = new Uri($"{_configuration[SettingKeys.AzureStorageCdn]}/{BlobContainers.CreatorRewards}/{reward.Filename}").OriginalString;
                    mappedRewards.Add(mappedReward);
                }

                return await Result<List<GetRewardItemDto>>.SuccessAsync(mappedRewards);
            }
        }
    }
}

using Application.Common.Constants;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.CreatorPortal.Creators.Dtos;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CreatorPortal.Creators.Queries.GetAll
{
    public class GetAllCreatorQuery : IRequest<Result<List<CreatorDto>>>
    {
        public string Query { get; set; }
        public int Take { get; set; } = 100;

        public class GetAllCreatorQueryHandler : IRequestHandler<GetAllCreatorQuery, Result<List<CreatorDto>>>
        {
            private readonly ICallContext _callContext;
            private readonly IMapper _mapper;
            private readonly IApplicationDbContext _dbContext;
            private readonly IConfiguration _configuration;

            public GetAllCreatorQueryHandler(IMapper mapper, IApplicationDbContext dbContext, ICallContext callContext, IConfiguration configuration)
            {
                _mapper = mapper;
                _dbContext = dbContext;
                _callContext = callContext;
                _configuration = configuration;
            }

            public async Task<Result<List<CreatorDto>>> Handle(GetAllCreatorQuery request, CancellationToken cancellationToken)
            {
                var query = request.Query.ToLower();
                var creators = await _dbContext.CreatorProfiles.AsQueryable()
                    .Where(x => string.IsNullOrEmpty(query) || x.Username.Contains(query))
                    .OrderByDescending(x => x.SubscriberCount)
                    .Take(request.Take)
                    .ToListAsync();

                var subscriptions = await _dbContext.CreatorSubscribers.AsQueryable()
                    .Where(x => x.SubscriberId == _callContext.UserId)
                    .ToListAsync();

                var mappedCreators = new List<CreatorDto>();

                foreach (var creator in creators)
                {
                    var mappedCreator = _mapper.Map<CreatorDto>(creator);

                    string profilePictureLink = "assets/default_photo.jpg";
                    if (!string.IsNullOrEmpty(creator.ProfilePictureFilename))
                    {
                        profilePictureLink = new Uri($"{_configuration[SettingKeys.AzureStorageCdn]}/{BlobContainers.Creators}/{creator.ProfilePictureFilename}").OriginalString;
                    }

                    mappedCreator.ProfilePictureLink = profilePictureLink;
                    mappedCreator.IsSubscribed = subscriptions.Any(x => x.CreatorId == creator.Id);
                    mappedCreators.Add(mappedCreator);
                }
                return await Result<List<CreatorDto>>.SuccessAsync(mappedCreators);
            }
        }
    }
}

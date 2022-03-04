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

namespace Application.CreatorPortal.CreatorSubscribers.Queries.GetAll
{
    public class GetAllSubscribersQuery : IRequest<Result<List<SubscriberDto>>>
    {
        public class GetAllSubscribersQueryHandler : IRequestHandler<GetAllSubscribersQuery, Result<List<SubscriberDto>>>
        {
            private readonly ICallContext _callContext;
            private readonly IMapper _mapper;
            private readonly IApplicationDbContext _dbContext;
            private readonly IConfiguration _configuration;

            public GetAllSubscribersQueryHandler(IMapper mapper, IApplicationDbContext dbContext, ICallContext callContext, IConfiguration configuration)
            {
                _mapper = mapper;
                _dbContext = dbContext;
                _callContext = callContext;
                _configuration = configuration;
            }

            public async Task<Result<List<SubscriberDto>>> Handle(GetAllSubscribersQuery request, CancellationToken cancellationToken)
            {
                var subscribers = await _dbContext.Subscribers.AsQueryable()
                        .Where(x => x.SubscriberIsAccountValid && !x.SubscriberIsAdmin && x.CreatorId == _callContext.UserId)
                        .ToListAsync();

                var mappedSubscribers = new List<SubscriberDto>();

                foreach (var subscriber in subscribers)
                {
                    var mappedSubscriber = new SubscriberDto();

                    string profilePictureLink = "assets/default_photo.jpg";
                    if (!string.IsNullOrEmpty(subscriber.SubscriberProfilePictureFilename))
                    {
                        profilePictureLink = new Uri($"{_configuration[SettingKeys.AzureStorageCdn]}/{BlobContainers.Creators}/{subscriber.SubscriberProfilePictureFilename}").OriginalString;
                    }

                    mappedSubscriber.Username = subscriber.SubscriberUsername;
                    mappedSubscriber.ProfilePictureLink = profilePictureLink;
                    mappedSubscribers.Add(mappedSubscriber);
                }

                return await Result<List<SubscriberDto>>.SuccessAsync(mappedSubscribers);
            }
        }
    }
}

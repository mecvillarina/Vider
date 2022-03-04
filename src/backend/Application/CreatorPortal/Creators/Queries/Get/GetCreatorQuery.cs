using Application.Common.Constants;
using Application.Common.Extensions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.CreatorPortal.Creators.Dtos;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CreatorPortal.Creators.Queries.Get
{
    public class GetCreatorQuery : IRequest<Result<CreatorDto>>
    {
        public string Username { get; set; }

        public class GetCreatorQueryHandler : IRequestHandler<GetCreatorQuery, Result<CreatorDto>>
        {
            private readonly ICallContext _callContext;
            private readonly IMapper _mapper;
            private readonly IApplicationDbContext _dbContext;
            private readonly IConfiguration _configuration;

            public GetCreatorQueryHandler(IMapper mapper, IApplicationDbContext dbContext, ICallContext callContext, IConfiguration configuration)
            {
                _mapper = mapper;
                _dbContext = dbContext;
                _callContext = callContext;
                _configuration = configuration;
            }

            public async Task<Result<CreatorDto>> Handle(GetCreatorQuery request, CancellationToken cancellationToken)
            {
                var creator = _dbContext.CreatorProfiles.AsQueryable().FirstOrDefault(x => !x.IsAdmin && x.UsernameNormalize == request.Username.ToNormalize());

                if (creator == null) return await Result<CreatorDto>.FailAsync("Creator is not exists.");

                string profilePictureLink = "assets/default_photo.jpg";
                if (!string.IsNullOrEmpty(creator.ProfilePictureFilename))
                {
                    profilePictureLink = new Uri($"{_configuration[SettingKeys.AzureStorageCdn]}/{BlobContainers.Creators}/{creator.ProfilePictureFilename}").OriginalString;
                }

                bool isSubscribed = await _dbContext.CreatorSubscribers.AsQueryable().AnyAsync(x => x.CreatorId == creator.Id && x.SubscriberId == _callContext.UserId);

                var mappedCreator = _mapper.Map<CreatorDto>(creator);
                mappedCreator.ProfilePictureLink = profilePictureLink;
                mappedCreator.IsSubscribed = isSubscribed;
                return await Result<CreatorDto>.SuccessAsync(mappedCreator);
            }
        }
    }
}

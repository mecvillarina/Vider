using Application.Common.Constants;
using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CreatorPortal.Account.Queries.MyProfile
{
    public class MyProfileQuery : IRequest<Result<MyProfileResponse>>
    {
        public class GetProfileQueryHandler : IRequestHandler<MyProfileQuery, Result<MyProfileResponse>>
        {
            private readonly ICallContext _context;
            private readonly IApplicationDbContext _dbContext;
            private readonly IMapper _mapper;
            private readonly IConfiguration _configuration;
            public GetProfileQueryHandler(ICallContext context, IMapper mapper, IApplicationDbContext dbContext, IConfiguration configuration)
            {
                _context = context;
                _mapper = mapper;
                _dbContext = dbContext;
                _configuration = configuration;
            }

            public async Task<Result<MyProfileResponse>> Handle(MyProfileQuery request, CancellationToken cancellationToken)
            {
                var creator = await _dbContext.CreatorProfiles.AsQueryable().FirstOrDefaultAsync(x => x.Id == _context.UserId);

                if (creator == null) return await Result<MyProfileResponse>.FailAsync("Creator is not exists.");

                string profilePictureLink = "assets/default_photo.jpg";
                if (!string.IsNullOrEmpty(creator.ProfilePictureFilename))
                {
                    profilePictureLink = new Uri($"{_configuration[SettingKeys.AzureStorageCdn]}/{BlobContainers.Creators}/{creator.ProfilePictureFilename}").OriginalString;
                }

                var mappedCreator = _mapper.Map<MyProfileResponse>(creator);
                mappedCreator.ProfilePictureLink = profilePictureLink;
                return await Result<MyProfileResponse>.SuccessAsync(mappedCreator);
            }
        }
    }
}

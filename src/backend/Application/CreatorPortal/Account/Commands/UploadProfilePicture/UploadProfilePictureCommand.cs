using Application.Common.Constants;
using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CreatorPortal.Account.Commands.UploadProfilePicture
{
    public class UploadProfilePictureCommand : IRequest<IResult>
    {
        public Stream FileStream { get; set; }
        public string FileExtension { get; set; }

        public class UploadProfilePictureCommandHandler : IRequestHandler<UploadProfilePictureCommand, IResult>
        {
            private readonly ICallContext _context;
            private readonly ICreatorIdentityService _identityService;
            private readonly IAzureStorageBlobService _blobService;
            public UploadProfilePictureCommandHandler(ICallContext context, ICreatorIdentityService identityService, IAzureStorageBlobService blobService)
            {
                _context = context;
                _identityService = identityService;
                _blobService = blobService;
            }

            public async Task<IResult> Handle(UploadProfilePictureCommand request, CancellationToken cancellationToken)
            {
                if (!AppConstants.AllowableNFTFormats.Split(",").Any(x => x.Trim() == request.FileExtension.ToLower()))
                {
                    return await Result.FailAsync("Image format is not supported.");
                }

                string filename = $"{Guid.NewGuid()}{request.FileExtension}".ToLower();
                await _blobService.UploadAsync(request.FileStream, BlobContainers.Creators, filename);

                return await _identityService.UpdateProfilePicture(_context.UserId, filename);
            }
        }
    }
}

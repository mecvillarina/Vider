using Application.Common.Constants;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using MediatR;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CreatorPortal.CreatorRewards.Commands.Upload
{
    public class UploadRewardCommand : IRequest<IResult>
    {
        public string Name { get; set; }
        public string Message { get; set; }
        public Stream FileStream { get; set; }
        public string FileExtension { get; set; }

        public class UploadRewardCommandHandler : IRequestHandler<UploadRewardCommand, IResult>
        {
            private readonly ICallContext _context;
            private readonly IAzureStorageBlobService _blobService;
            private readonly IApplicationDbContext _dbContext;
            public UploadRewardCommandHandler(ICallContext context, IAzureStorageBlobService blobService, IApplicationDbContext dbContext)
            {
                _context = context;
                _blobService = blobService;
                _dbContext = dbContext;
            }

            public async Task<IResult> Handle(UploadRewardCommand request, CancellationToken cancellationToken)
            {
                if (!AppConstants.AllowableNFTFormats.Split(",").Any(x => x.Trim() == request.FileExtension.ToLower()))
                {
                    return await Result.FailAsync("Format not supported.");
                }

                string filename = $"{Guid.NewGuid()}{request.FileExtension}".ToLower();
                await _blobService.UploadAsync(request.FileStream, BlobContainers.CreatorRewards, filename);

                var taxon = new Random().Next(100, 1000000);

                _dbContext.CreatorRewards.Add(new CreatorReward()
                {
                    CreatorId = _context.UserId,
                    Filename = filename,
                    Taxon = taxon,
                    Name = request.Name,
                    Message = request.Message
                });

                await _dbContext.SaveChangesAsync();

                return await Result.SuccessAsync();
            }
        }
    }
}

using Application.Common.Constants;
using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CreatorPortal.Creators.Commands.UploadDump
{
    public class UploadDumpCommand : IRequest<Result<string>>
    {
        public Stream FileStream { get; set; }
        public string FileExtension { get; set; }

        public class UploadDumpCommandHandler : IRequestHandler<UploadDumpCommand, Result<string>>
        {
            private readonly IAzureStorageBlobService _blobService;
            private readonly IConfiguration _configuration;
            public UploadDumpCommandHandler(IAzureStorageBlobService blobService, IConfiguration configuration)
            {
                _blobService = blobService;
                _configuration = configuration;
            }

            public async Task<Result<string>> Handle(UploadDumpCommand request, CancellationToken cancellationToken)
            {
                if (!AppConstants.AllowableNFTFormats.Split(",").Any(x => x.Trim() == request.FileExtension.ToLower()))
                {
                    return await Result<string>.FailAsync("Format not supported.");
                }

                string filename = $"{Guid.NewGuid()}{request.FileExtension}".ToLower();
                await _blobService.UploadAsync(request.FileStream, BlobContainers.Dump, filename);
                var urlLink = new Uri($"{_configuration[SettingKeys.AzureStorageCdn]}/{BlobContainers.Dump}/{filename}").OriginalString;
                return await Result<string>.SuccessAsync(data: urlLink);
            }
        }
    }
}

using Application.Common.Models;
using Application.CreatorPortal.Creators.Commands.UploadDump;
using Application.CreatorPortal.Creators.Dtos;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.WebServices
{
    public interface ICreatorWebService : IWebService
    {
        Task<IResult<CreatorDto>> GetProfileAsync(string username, string accessToken);
        Task<IResult<List<CreatorDto>>> SearchAsync(string query, int take, string accessToken);
        Task<IResult<string>> UploadDumpAsync(UploadDumpCommand request, Stream fileStream, string filename, string accessToken);
    }
}
using Application.Common.Models;
using Application.CreatorPortal.Creators.Dtos;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.Managers
{
    public interface ICreatorManager : IManager
    {
        Task<IResult<CreatorDto>> GetProfileAsync(string username);
        Task<IResult<List<CreatorDto>>> SearchAsync(string query, int take);
        Task<IResult<string>> UploadDumpAsync(Stream fileStream, string filename);
    }
}
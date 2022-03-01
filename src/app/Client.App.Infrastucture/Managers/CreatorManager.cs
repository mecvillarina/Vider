using Application.Common.Models;
using Application.CreatorPortal.Creators.Commands.UploadDump;
using Application.CreatorPortal.Creators.Dtos;
using Client.App.Infrastructure.WebServices;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.Managers
{
    public class CreatorManager : ManagerBase, ICreatorManager
    {
        private readonly ICreatorWebService _creatorWebService;
        public CreatorManager(IManagerToolkit managerToolkit, ICreatorWebService creatorWebService) : base(managerToolkit)
        {
            _creatorWebService = creatorWebService;
        }

        public async Task<IResult<CreatorDto>> GetProfileAsync(string username)
        {
            await PrepareForWebserviceCall();
            return await _creatorWebService.GetProfileAsync(username, AccessToken);
        }

        public async Task<IResult<List<CreatorDto>>> SearchAsync(string query, int take)
        {
            await PrepareForWebserviceCall();
            return await _creatorWebService.SearchAsync(query, take, AccessToken);
        }

        public async Task<IResult<string>> UploadDumpAsync(Stream fileStream, string filename)
        {
            await PrepareForWebserviceCall();
            return await _creatorWebService.UploadDumpAsync(new UploadDumpCommand(), fileStream, filename, AccessToken);
        }
    }
}

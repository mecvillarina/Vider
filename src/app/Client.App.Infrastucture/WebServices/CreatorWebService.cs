using Application.Common.Models;
using Application.CreatorPortal.Creators.Commands.UploadDump;
using Application.CreatorPortal.Creators.Dtos;
using Client.App.Infrastructure.Routes;
using Client.App.Infrastructure.WebServices.Base;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.WebServices
{
    public class CreatorWebService : WebServiceBase, ICreatorWebService
    {
        public CreatorWebService(AppHttpClient appHttpClient) : base(appHttpClient)
        {
        }

        public Task<IResult<CreatorDto>> GetProfileAsync(string username, string accessToken) => GetAsync<CreatorDto>(string.Format(CreatorEndpoints.GetProfile, username), accessToken);
        public Task<IResult<List<CreatorDto>>> SearchAsync(string query, int take, string accessToken) => GetAsync<List<CreatorDto>>(string.Format(CreatorEndpoints.Search, query, take), accessToken);
        public Task<IResult<string>> UploadDumpAsync(UploadDumpCommand request, Stream fileStream, string filename, string accessToken) => PostFileAsync<UploadDumpCommand, string>(CreatorEndpoints.UploadDump, request, fileStream, filename, accessToken);
    }
}

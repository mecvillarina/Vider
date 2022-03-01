using Application.Common.Models;
using Application.CreatorPortal.Creators.Dtos;
using Application.CreatorPortal.CreatorSubscribers.Commands.Subscribe;
using Client.App.Infrastructure.Routes;
using Client.App.Infrastructure.WebServices.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.WebServices
{
    public class CreatorSubscriberWebService : WebServiceBase, ICreatorSubscriberWebService
    {
        public CreatorSubscriberWebService(AppHttpClient appHttpClient) : base(appHttpClient)
        {
        }

        public Task<IResult<int>> SubscribeAsync(SubscribeCommand request, string accessToken) => PostAsync<SubscribeCommand, int>(CreatorSubscriberEndpoints.Subscribe, request, accessToken);
        public Task<IResult<List<SubscriberDto>>> GetSubscribersAsync(string accessToken) => GetAsync<List<SubscriberDto>>(CreatorSubscriberEndpoints.GetSubscribers, accessToken);

    }
}

using Application.Common.Models;
using Application.CreatorPortal.Creators.Dtos;
using Application.CreatorPortal.CreatorSubscribers.Commands.Subscribe;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.WebServices
{
    public interface ICreatorSubscriberWebService : IWebService
    {
        Task<IResult<int>> SubscribeAsync(SubscribeCommand request, string accessToken);
        Task<IResult<List<SubscriberDto>>> GetSubscribersAsync(string accessToken);
    }
}
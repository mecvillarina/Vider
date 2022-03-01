using Application.Common.Models;
using Application.CreatorPortal.Creators.Dtos;
using Application.CreatorPortal.CreatorSubscribers.Commands.Subscribe;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.Managers
{
    public interface ICreatorSubscriberManager : IManager
    {
        Task<IResult<int>> SubscribeAsync(SubscribeCommand request);
        Task<IResult<List<SubscriberDto>>> GetSubscribersAsync();
    }
}
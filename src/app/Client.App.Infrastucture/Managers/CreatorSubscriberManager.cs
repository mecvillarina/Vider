using Application.Common.Models;
using Application.CreatorPortal.Creators.Dtos;
using Application.CreatorPortal.CreatorSubscribers.Commands.Subscribe;
using Client.App.Infrastructure.WebServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.Managers
{
    public class CreatorSubscriberManager : ManagerBase, ICreatorSubscriberManager
    {
        private readonly ICreatorSubscriberWebService _creatorSubscriberWebService;
        public CreatorSubscriberManager(IManagerToolkit managerToolkit, ICreatorSubscriberWebService creatorSubscriberWebService) : base(managerToolkit)
        {
            _creatorSubscriberWebService = creatorSubscriberWebService;
        }

        public async Task<IResult<int>> SubscribeAsync(SubscribeCommand request)
        {
            await PrepareForWebserviceCall();
            return await _creatorSubscriberWebService.SubscribeAsync(request, AccessToken);
        }

        public async Task<IResult<List<SubscriberDto>>> GetSubscribersAsync()
        {
            await PrepareForWebserviceCall();
            return await _creatorSubscriberWebService.GetSubscribersAsync(AccessToken);
        }
    }
}

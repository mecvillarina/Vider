using Application.Common.Mappings;
using Domain.Entities;
using System;

namespace Application.CreatorPortal.Creators.Dtos
{
    public class SubscriberDto : IMapFrom<Subscriber>
    {
        public string Username { get; set; }
        public string ProfilePictureLink { get; set; }
        public DateTime DateSubscribed { get; set; }
    }
}

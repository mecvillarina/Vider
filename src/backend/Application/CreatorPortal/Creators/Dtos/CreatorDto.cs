using Application.Common.Mappings;
using Domain.Entities;
using System;

namespace Application.CreatorPortal.Creators.Dtos
{
    public class CreatorDto : IMapFrom<CreatorProfile>
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Bio { get; set; }
        public DateTime DateRegistered { get; set; }
        public string ProfilePictureLink { get; set; }
        public int SubscriberCount { get; set; }
        public bool IsSubscribed { get; set; }
    }
}

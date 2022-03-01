using Application.Common.Mappings;
using Domain.Entities;
using System;

namespace Application.CreatorPortal.Account.Queries.MyProfile
{
    public class MyProfileResponse : IMapFrom<CreatorProfile>
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Bio { get; set; }
        public string ProfilePictureLink { get; set; }
        public DateTime DateRegistered { get; set; }
        public int SubscriberCount { get; set; }
    }
}

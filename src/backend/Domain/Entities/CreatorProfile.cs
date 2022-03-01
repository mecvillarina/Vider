using System;

namespace Domain.Entities
{
    public class CreatorProfile
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string UsernameNormalize { get; set; }
        public string Name { get; set; }
        public string Bio { get; set; }
        public string ProfilePictureFilename { get; set; }
        public DateTime DateRegistered { get; set; }
        public int SubscriberCount { get; set; }
    }
}

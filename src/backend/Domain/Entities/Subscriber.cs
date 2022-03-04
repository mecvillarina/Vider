namespace Domain.Entities
{
    public class Subscriber
    {
        public int Id { get; set; }
        public int CreatorId { get; set; }
        public string SubscriberUsername { get; set; }
        public string SubscriberProfilePictureFilename { get; set; }
        public bool SubscriberIsAccountValid { get; set; }
        public bool SubscriberIsAdmin { get; set; }
    }
}

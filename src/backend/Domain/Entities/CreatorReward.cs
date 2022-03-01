namespace Domain.Entities
{
    public class CreatorReward
    {
        public int Id { get; set; }

        public int CreatorId { get; set; }
        public virtual Creator Creator { get; set; }

        public int Taxon { get; set; }
        public string Filename { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }
    }
}

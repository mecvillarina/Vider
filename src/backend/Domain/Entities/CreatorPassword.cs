namespace Domain.Entities
{
    public class CreatorPassword
    {
        public int Id { get; set; }

        public int CreatorId { get; set; }
        public virtual Creator Creator { get; set; }

        public string Salt { get; set; }

        public string Digest { get; set; }

        public bool IsDeleted { get; set; }
    }
}

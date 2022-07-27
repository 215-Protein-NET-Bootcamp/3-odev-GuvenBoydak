namespace JwtHomework.Entities
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public DateTime? DeletedDate { get; set; }

        public DataStatus Status { get; set; }

        public BaseEntity()
        {
            CreatedDate = DateTime.Now;
            Status = DataStatus.Insearted;
        }
    }
}

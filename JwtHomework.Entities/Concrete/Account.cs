namespace JwtHomework.Entities
{
    public class Account : BaseEntity
    {
        public string UserName { get; set; }

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }

        public DateTime LastActivity { get; set; }

        //Relational Property
        public List<Person> People { get; set; }
    }
}

namespace JwtHomework.Entities
{
    public class AccountDto
    {
        public string UserName { get; set; }

        public byte[] PasswordHash { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }

        public DateTime LastActivity { get; set; }
    }
}

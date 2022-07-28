namespace JwtHomework.Entities
{
    public class AccountPasswordUpdateDto
    {

        public string OldPassword { get; set; }

        public string NewPassword { get; set; }
        
        public string ConfirmPassword { get; set; }

    }
}

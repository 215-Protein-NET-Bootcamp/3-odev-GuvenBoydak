namespace JwtHomework.Base
{
    public class AccessToken
    {
        public string Token { get; set; }//kulanıcıya vericegimiz token degeri

        public DateTime Expiration { get; set; } //Verilen token süresini tutuyoruz
    }
}

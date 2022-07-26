using Microsoft.IdentityModel.Tokens;

namespace JwtHomework.Base
{
    public class SigningCredentialHelper
    {
        public static SigningCredentials CreateSigninCredentials(SecurityKey securityKey)
        {
            //Güvenlik anahtari olarak securityKey'i, şifreleme algoritmesi olarakta HmacSha512Signature veriyoruz.
            return new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);
        }
    }
}

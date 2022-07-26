using JwtHomework.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace JwtHomework.Base
{
    public class JWTHelper : ITokenHelper
    {
        public IConfiguration Configuration { get;}

        //TokenOptions içerisindeki propertyleri doldurmak için injection yapıyoruz.
        private TokenOptions _tokenOptions;

        private DateTime _accessTokenExpiration;

        public JWTHelper(IConfiguration configuration)
        {
            Configuration = configuration;
            //Configuration.GetSection("TokenOptions") ile appsettings.json içerisindeki TokenOptions degerlerini Get<TokenOptions> sınıfındaki degerlere atıyoruz(Mapliyoruz).
            _tokenOptions = Configuration.GetSection("TokenOptions").Get<TokenOptions>();
        }

        public AccessToken CreateToken(Account account)
        {
            //Token bitiş süresini veriyoruz DateTime.Now.AddMinutes ile oluştugu andan itibaren _tokenOptions.AccessTokenExpiration dan gelen süreyi atıyoruz.
            _accessTokenExpiration = DateTime.Now.AddMinutes(_tokenOptions.AccessTokenExpiration);

            //_tokenOptions.SecurityKey ile  TokenOptions icerisindeki SecurityKey'i veriyoruz ve byte array olarak bir SecurityKey oluşturuluyor.
            SecurityKey securityKey = SecurityKeyHelper.CreateSecurityKey(_tokenOptions.SecurityKey);

            //bir üstte oluştrulan securtiyKey veriyoruz ve bize bir creadentials oluşturuyor.
            SigningCredentials signingCredentials = SigningCredentialHelper.CreateSigninCredentials(securityKey);

            //Token olusturmak için gerekli olan parametreleri veriyoruz ve bize bir Token oluşturuyor.
            JwtSecurityToken jwt = CreateJwtSecurityToken(_tokenOptions, account, signingCredentials);

            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            //jwt atadıgımız degerlerle bir jwt token oluşturuyoruz.
            string token = jwtSecurityTokenHandler.WriteToken(jwt);

            return new AccessToken {
            Token=token,
            Expiration=_accessTokenExpiration,
            };
        }

        //JWT oluşturdugumuz method. Parametre olarak verdigimiz TokenOptins,user,signingCredentials  vererek bir JWT oluşturuyoruz.
        public JwtSecurityToken CreateJwtSecurityToken(TokenOptions tokenOptions,Account account,SigningCredentials signingCredentials)
        {
            JwtSecurityToken jwt = new JwtSecurityToken(
                issuer: tokenOptions.Issuer,
                audience: tokenOptions.Audience,
                expires: _accessTokenExpiration,
                notBefore: DateTime.Now,
                claims: GetClaim(account),
                signingCredentials: signingCredentials
                );
            return jwt;
        }

        private static Claim[] GetClaim(Account account)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()),
                new Claim(ClaimTypes.Name, account.UserName),
                new Claim("AccountId", account.Id.ToString()),
            };

            return claims;
        }
    }
}

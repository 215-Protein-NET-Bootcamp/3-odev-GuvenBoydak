using JwtHomework.Entities;

namespace JwtHomework.Base
{
    public interface ITokenHelper
    {
        AccessToken CreateToken(Account account);
    }
}

using JwtHomework.Base;
using JwtHomework.Entities;

namespace JwtHomework.Business
{
    public interface IAccountService:IService<Account>
    {
        Task<CustomResponseDto<Account>> RegisterAsync(AccountRegisterDto entity);

        Task<CustomResponseDto<Account>> Login(AccountLoginDto entity);

        Task<CustomResponseDto<Account>> GetByUserAsync(string userName);

        Task<CustomResponseDto<AccessToken>> CreateAccessToken(Account entity);
    }
}

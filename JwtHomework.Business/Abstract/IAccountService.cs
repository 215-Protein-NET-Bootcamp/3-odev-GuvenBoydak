
using JwtHomework.Base;
using JwtHomework.Entities;

namespace JwtHomework.Business
{
    public interface IAccountService:IService<Account>
    {
        Task<Account> RegisterAsync(AccountRegisterDto entity);

        Task<Account> LoginAsync(AccountLoginDto entity);

        Task<Account> GetByUserAsync(string userName);

        Task<AccessToken> CreateAccessTokenAsync(Account entity);
    }
}

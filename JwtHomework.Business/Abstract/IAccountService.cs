
using JwtHomework.Base;
using JwtHomework.Entities;

namespace JwtHomework.Business
{
    public interface IAccountService:IService<Account>
    {
        Task RegisterAsync(AccountRegisterDto entity);

        Task LoginAsync(AccountLoginDto entity);

        Task<Account> GetByUserAsync(string userName);

        Task<AccessToken> CreateAccessTokenAsync(Account entity);
    }
}

using JwtHomework.Entities;

namespace JwtHomework.DataAccess
{
    public interface IAccountRepository:IRepository<Account>
    {
        Task<Account> GetByUserAsync(string userName);

    }
}

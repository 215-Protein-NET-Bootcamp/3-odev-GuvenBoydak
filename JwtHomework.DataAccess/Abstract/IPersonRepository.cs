using JwtHomework.Entities;
using System.Collections.Generic;

namespace JwtHomework.DataAccess
{
    public interface IPersonRepository:IRepository<Person>
    {
        Task<IEnumerable<Person>> GetByAccountIdAsync(int id);

        Task<List<Person>> GetPaginationAsync(int page, int limit);
    }
}

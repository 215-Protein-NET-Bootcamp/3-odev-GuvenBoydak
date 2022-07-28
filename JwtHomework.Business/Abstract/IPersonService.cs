using JwtHomework.Entities;

namespace JwtHomework.Business
{
    public interface IPersonService:IService<Person>
    {
        Task<IEnumerable<Person>> GetByAccountIdAsync(int id);

    }
}

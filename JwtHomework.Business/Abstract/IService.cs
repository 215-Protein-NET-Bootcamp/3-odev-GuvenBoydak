namespace JwtHomework.Business
{
    public interface IService<T> where T : class
    {
        Task<T> GetByIdAsync(int id);

        Task<List<T>> GetAllAsync();

        Task<List<T>> GetActivesAsync();

        Task InsertAsync(T entity);

        Task UpdateAsync(T entity);

        Task RemoveAsync(T entity);
    }
}

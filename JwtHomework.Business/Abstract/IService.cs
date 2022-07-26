using JwtHomework.Base;
using JwtHomework.Entities;

namespace JwtHomework.Business
{
    public interface IService<T> where T : BaseEntity
    {
        Task<CustomResponseDto<T>> GetByIdAsync(int id);

        Task<CustomResponseDto<IEnumerable<T>>> GetAllAsync();

        Task<CustomResponseDto<IEnumerable<T>>> GetActivesAsync();

        Task<CustomResponseDto<T>> InsertAsync(T entity);

        Task<CustomResponseDto<T>> UpdateAsync(T entity);

        Task<CustomResponseDto<T>> RemoveAsync(T entity);
    }
}

﻿using JwtHomework.Entities;

namespace JwtHomework.DataAccess
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAllAsync();

        Task<IEnumerable<T>> GetActiveAsync();

        Task<T> GetByIdAsync(int id);

        Task AddAsync(T entity);

        Task DeleteAsync(T entity);

        Task UpdateAsync(T entity);

    }
}

﻿using AlHantoushi.Core.Entities;
using AlHantoushi.Core.Specifications;

namespace AlHantoushi.Core.Interfaces;

public interface IGenericRepository<T> where T : BaseEntity
{
    Task<T?> GetByIdAsync(int id);
    Task<IReadOnlyList<T>> ListAllAsync();
    Task<T?> GetEntityWithSpec(ISpecification<T> spec);
    Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec);
    void Add(T entity);
    void Update(T entity);
    Task<int> Remove(int id, ISpecification<T> spec);
    void Remove(T entity);
    Task<bool> SaveAllAsync();
    bool Exists(int id);
    Task<int> CountAsync(ISpecification<T> spec);
}

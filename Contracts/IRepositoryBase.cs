﻿using System.Linq.Expressions;

namespace Contracts;

public interface IRepositoryBase<T>
{
    IQueryable<T> FindAll(bool trackChanges);

    IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges);

    void Create(T entity);

    void CreateCollection(IEnumerable<T> entities);

    void Update(T entity);

    void Delete(T entity);
}
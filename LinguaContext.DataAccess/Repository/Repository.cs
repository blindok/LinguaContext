using LinguaContext.DataAccess.Repository.Interfaces;
using LinguaContext.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace LinguaContext.DataAccess.Repository;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly ApplicationDbContext _db;
    internal  readonly DbSet<T> _dbSet;

    public Repository(ApplicationDbContext db)
    {
        _db = db;
        _dbSet = _db.Set<T>();
    }

    public IAsyncEnumerable<T> GetAll(string? includeProperties = null)
    {
        IQueryable<T> query = _dbSet;

        if (!string.IsNullOrEmpty(includeProperties))
            IncludeProperties(query, includeProperties);

        return query.AsAsyncEnumerable();
    }

    public T? GetFirstOrDefault(Expression<Func<T, bool>> filter, string? includeProperties = null)
    {
        IQueryable<T> query = _dbSet.Where(filter);

        if (!string.IsNullOrEmpty(includeProperties))
            IncludeProperties(query, includeProperties);

        return query.FirstOrDefault();
    }

    public void Create(T entity)
    {
        _dbSet.Add(entity);
    }

    public void Update(T entity) 
    { 
        _dbSet.Update(entity);
    }

    public void Delete(T entity) 
    {
        _dbSet.Remove(entity);
    }

    public void IncludeProperties(IQueryable<T> query, string includeProperties)
    {
        foreach (var property in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
        {
            query.Include(property);
        }
    }
}

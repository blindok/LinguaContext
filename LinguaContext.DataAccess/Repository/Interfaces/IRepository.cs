using System.Linq.Expressions;

namespace LinguaContext.DataAccess.Repository.Interfaces;

public interface IRepository<T> where T : class
{
    IEnumerable<T> GetAll(string? includeProperties = null);
    T? GetFirstOrDefault(Expression<Func<T, bool>> filter, string? includeProperties = null);
    void Create(T entity);
    void Update(T entity);
    void Delete(T entity);
    void IncludeProperties(IQueryable<T> query, string includeProperties);
}

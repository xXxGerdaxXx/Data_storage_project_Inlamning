using System.Linq.Expressions;

namespace Data_storage_project_library.Interfaces;

public interface IBaseRepository<T> where T : class
{
    Task<T?> CreateAsync(T entity);
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetAsync(Expression<Func<T, bool>> expression);
    Task<T?> UpdateAsync(T entity, Expression<Func<T, bool>> identifierExpression);
    Task<bool> DeleteAsync(Expression<Func<T, bool>> expression);

}

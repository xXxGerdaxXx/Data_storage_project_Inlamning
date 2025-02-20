using System.Diagnostics;
using System.Linq.Expressions;
using Data_storage_project_library.Contexts;
using Data_storage_project_library.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data_storage_project_library.Repositories;

public  class BaseRepository<T>(ApplicationDbContext context, ILoggerService logger) : IBaseRepository<T> where T : class
{
    protected readonly ApplicationDbContext _context = context;
    protected readonly DbSet<T> _dbSet = context.Set<T>();
    private readonly ILoggerService _logger = logger;

    public async Task<T?> CreateAsync(T entity)
    {
        if (entity == null) return null;

        try
        {
            await _dbSet.AddAsync(entity);
            return entity;
        }
        catch (Exception ex)
        {
            _logger.LogError("Error creating entity", ex);
            return null;
        }
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    /// <summary>
    /// Fetches a single record that matches the condition, with the option to include related data.
    /// This allows me to fetch things like an employee with their role or a service with its currency 
    /// in a single query instead of making multiple database calls. 
    /// </summary>

    public async Task<T?> GetAsync(Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[] includeProperties)
    {
        try
        {
            IQueryable<T> query = _dbSet;

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return await query.FirstOrDefaultAsync(expression);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error fetching entity: {typeof(T).Name}", ex);  
            return null;  
        }
    }


    public async Task<T?> UpdateAsync(T entity, Expression<Func<T, bool>> identifierExpression)
    {
        if (entity == null || identifierExpression == null) return null;

        try
        {
            var existingEntity = await GetAsync(identifierExpression);
            if (existingEntity == null) return null;

            _context.Entry(existingEntity).CurrentValues.SetValues(entity);
            return existingEntity;
        }
        catch (Exception ex)
        {
            _logger.LogError("Error updating entity", ex);
            return null;
        }
    }

    public async Task<bool> DeleteAsync(Expression<Func<T, bool>> expression)
    {
        if (expression == null) return false;

        try
        {
            var existingEntity = await GetAsync(expression);
            if (existingEntity == null) return false;

            _dbSet.Remove(existingEntity);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError("Error deleting entity", ex);
            return false;
        }
    }
}

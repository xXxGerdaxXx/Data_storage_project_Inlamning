using System.Diagnostics;
using System.Linq.Expressions;
using Data_storage_project_library.Contexts;
using Data_storage_project_library.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data_storage_project_library.Repositories;

public class BaseRepository<T> : IBaseRepository<T> where T : class
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<T> _dbSet;

    public BaseRepository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }


    public async Task<T?> CreateAsync(T entity)
    {
        if (entity == null) return null;

        try
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error creating entity: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// I used ChatGTP 4o to help me with eager loading and these two methods bellow.

    /// I use eager loading here to avoid multiple queries when fetching related data like 
    /// an employee’s role, a project’s status, or a service’s currency. This loads everything 
    /// in one go instead of making separate database calls.

    /// For example:
    /// - In EmployeeRepository, I include the employee's role.
    /// - In ServiceRepository, I include the service’s currency.
    /// - In ProjectRepository, I could include the project’s status or assigned employee.
    /// </summary>

    //public async Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includeProperties)
    //{
    //    IQueryable<T> query = _dbSet;

    //    foreach (var includeProperty in includeProperties)
    //    {
    //        query = query.Include(includeProperty); 
    //    }

    //    return await query.ToListAsync();
    //}


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
        IQueryable<T> query = _dbSet;

        foreach (var includeProperty in includeProperties)
        {
            query = query.Include(includeProperty); 
        }

        return await query.FirstOrDefaultAsync(expression);
    }

    public async Task<T?> UpdateAsync(T entity, Expression<Func<T, bool>> identifierExpression)
    {
        if (entity == null || identifierExpression == null) return null;

        try
        {
            var existingEntity = await GetAsync(identifierExpression);
            if (existingEntity == null) return null;

            _context.Entry(existingEntity).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();
            return existingEntity;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error updating entity: {ex.Message}");
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
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error deleting entity: {ex.Message}");
            return false;
        }
    }
}

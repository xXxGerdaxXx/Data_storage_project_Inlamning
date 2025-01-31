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

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        try
        {
            return await _dbSet.ToListAsync();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error fetching all entities: {ex.Message}");
            return [];
        }
    }

    public async Task<T?> GetAsync(Expression<Func<T, bool>> expression)
    {
        if (expression == null) return null;

        try
        {
            return await _dbSet.FirstOrDefaultAsync(expression);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error fetching entity: {ex.Message}");
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

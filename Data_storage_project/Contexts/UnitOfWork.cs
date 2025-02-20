using Data_storage_project_library.Contexts;
using Data_storage_project_library.Interfaces;

namespace Data_storage_project_library.Contexts;

public class UnitOfWork(ApplicationDbContext context) : IUnitOfWork
{
    private readonly ApplicationDbContext _context = context;

    public async Task<T> ExecuteAsync<T>(Func<Task<T>> operation)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            T result = await operation();
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            return result;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}

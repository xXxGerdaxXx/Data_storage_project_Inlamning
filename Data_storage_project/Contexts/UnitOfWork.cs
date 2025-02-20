using Data_storage_project_library.Contexts;
using Data_storage_project_library.Interfaces;

/*
 * This `UnitOfWork` class was structured with the help of ChatGPT to ensure it follows best practices 
 * in transaction management and resource cleanup. I integrated the IDisposable interface to properly 
 * manage the lifecycle of the ApplicationDbContext.
 * 
 *  **Key Features:**
 *  **Transaction Management:** 
 *    - `ExecuteAsync<T>` ensures database operations are executed within a transaction.
 *    - It starts a transaction, executes the operation, commits on success, and rolls back on failure.
 *  
 *  **Implements IDisposable:**  
 *    - This ensures that `_context.Dispose()` is called when `UnitOfWork` is no longer needed.
 *    - `GC.SuppressFinalize(this)` prevents the garbage collector from calling the finalizer.
 *    - `_disposed` flag ensures `Dispose` is only executed once.
 *  
 *  **Prevents Resource Leaks:**  
 *    - Without proper disposal, the database context could keep open connections, leading to memory issues.
 *  
 *  **Why This Approach?**
 * - Following SOLID principles, `UnitOfWork` encapsulates transaction management, improving maintainability.
 * - By injecting `UnitOfWork` into services via Dependency Injection (DI), I ensure database operations are atomic.
 */


namespace Data_storage_project_library.Contexts;

public class UnitOfWork(ApplicationDbContext context) : IUnitOfWork, IDisposable
{
    private readonly ApplicationDbContext _context = context;
    private bool _disposed = false;

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


    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this); 
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _context.Dispose(); 
            }
            _disposed = true;
        }
    }
}

using Data_storage_project_library.Contexts; 
using Microsoft.EntityFrameworkCore; 
using Microsoft.EntityFrameworkCore.Design; 
using Microsoft.Extensions.Logging; 
namespace Data_storage_project_library.Contexts
{
    public class DataContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            // Configure SQL Server provider
            optionsBuilder.UseSqlServer(
                "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\PROJEKTAI\\Data_storage_project_solution\\Data_storage_project\\Databases\\local_data_base.mdf;Integrated Security=True;Connect Timeout=30",
                sqlOptions => sqlOptions.MigrationsAssembly("Data_storage_project_library") // Specify assembly containing migrations
            );

            // Optional: Enable detailed logging during design-time DbContext creation
            optionsBuilder.LogTo(Console.WriteLine, LogLevel.Warning)
                          .EnableSensitiveDataLogging(false);

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}

using Data_storage_project_library.Contexts;
using Data_storage_project_library.Interfaces;
using Data_storage_project_library.Repositories;
using Data_storage_project_library.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();

services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer("ConnectionStringHere")); // WILL ADD CONNECTIONSTRINGLATER

services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

services.AddScoped<ICustomerService, CustomerService>();
services.AddScoped<IProjectService, ProjectService>();
services.AddScoped<IEmployeeService, EmployeeService>();
services.AddScoped<IRoleService, RoleService>();
services.AddScoped<IStatusService, StatusService>();
services.AddScoped<ICurrencyService, CurrencyService>();

var serviceProvider = services.BuildServiceProvider();

using (var scope = serviceProvider.CreateScope())
{
    var customerService = scope.ServiceProvider.GetRequiredService<ICustomerService>();

    // Example
    Console.WriteLine("Fetching all customers...");
    var customers = await customerService.GetAllCustomersAsync();
    foreach (var customer in customers)
    {
        Console.WriteLine($"Customer: {customer.CustomerName}");
    }
}

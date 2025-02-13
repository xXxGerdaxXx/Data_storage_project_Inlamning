using Data_storage_project_library.Contexts;
using Data_storage_project_library.Entities;
using Data_storage_project_library.Interfaces;
using Data_storage_project_library.Repositories;
using Data_storage_project_library.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Presentation.MenuService.EntityMenus;


var services = new ServiceCollection()
    .AddDbContext<ApplicationDbContext>(x => x.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\PROJEKTAI\\Data_storage_project_solution\\Data_storage_project\\Databases\\local_data_base.mdf;Integrated Security=True;Connect Timeout=30"))
    .AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>))
    .AddScoped<ICustomerService, CustomerService>()
    .AddScoped<ProjectRepository>()
    .AddScoped<CustomerRepository>()
    .AddScoped<EmployeeRepository>()
    .AddScoped<RoleRepository>()
    .AddScoped<ServiceRepository>()
    .AddScoped<IEmployeeService, EmployeeService>()
    .AddScoped<IProjectService, ProjectService>()
    .AddScoped<IBaseRepository<ServiceEntity>, BaseRepository<ServiceEntity>>()
    .AddScoped<StatusService>() 
    .AddScoped<IStatusService, StatusService>()
    .AddScoped<ICurrencyService, CurrencyService>()
    .AddScoped<IServiceService, ServiceService>()
    .AddScoped<IRoleService, RoleService>()
    .AddScoped<MainMenu>()
    .AddScoped<CustomerMenu>()
    .AddScoped<EmployeeMenu>()
    .AddScoped<ProjectMenu>()
    .AddScoped<StatusMenu>()
    .AddScoped<CurrencyMenu>()
    .AddScoped<ServiceMenu>()
    .AddScoped<RoleMenu>()
    .BuildServiceProvider();

var menu = services.GetRequiredService<MainMenu>();
await menu.RunAsync();





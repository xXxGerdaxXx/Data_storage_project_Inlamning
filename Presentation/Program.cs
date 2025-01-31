using Data_storage_project_library.Contexts;
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
.AddScoped<IEmployeeService, EmployeeService>()
.AddScoped<IProjectService, ProjectService>()
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


//var host = Host.CreateDefaultBuilder(args)
//    .ConfigureLogging(logging =>
//    {
//        logging.ClearProviders();
//        logging.AddConsole();
//        logging.SetMinimumLevel(LogLevel.Warning);
//    })
//    .ConfigureServices((context, services) =>
//    {

//        services.AddDbContext<ApplicationDbContext>(options =>
//            options.UseSqlServer(
//                "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\PROJEKTAI\\Data_storage_project_solution\\Data_storage_project\\Databases\\local_data_base.mdf;Integrated Security=True;Connect Timeout=30",
//                sqlOptions => sqlOptions.MigrationsAssembly("Data_storage_project_library"))
//                .EnableSensitiveDataLogging(false)
//                .LogTo(Console.WriteLine, LogLevel.Warning));


//        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

//        services.AddScoped<ProjectRepository>();
//        services.AddScoped<CustomerRepository>();

//        services.AddScoped<ICustomerService, CustomerService>();
//        services.AddScoped<IEmployeeService, EmployeeService>();
//        services.AddScoped<IProjectService, ProjectService>();
//        services.AddScoped<IStatusService, StatusService>();
//        services.AddScoped<ICurrencyService, CurrencyService>();
//        services.AddScoped<IServiceService, ServiceService>();
//        services.AddScoped<IRoleService, RoleService>();


//        services.AddScoped<MainMenu>();
//        services.AddScoped<CustomerMenu>();
//        services.AddScoped<EmployeeMenu>();
//        services.AddScoped<ProjectMenu>();
//        services.AddScoped<StatusMenu>();
//        services.AddScoped<CurrencyMenu>();
//        services.AddScoped<ServiceMenu>();
//        services.AddScoped<RoleMenu>();
//    })
//    .Build();

//using var scope = host.Services.CreateScope();
//var services = scope.ServiceProvider;

//try
//{
//    var app = services.GetRequiredService<MainMenu>();
//    await app.RunAsync();
//}
//catch (Exception ex)
//{
//    Console.WriteLine($"An error occurred while running the application: {ex.Message}");
//    Console.WriteLine($"Stack Trace: {ex.StackTrace}");
//    Console.WriteLine("Inner Exception: " + ex.InnerException?.Message);
//}


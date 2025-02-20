using Data_storage_project_library.Repositories;
using Data_storage_project_library.Contexts;
using Data_storage_project_library.Interfaces;
using Data_storage_project_library.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();



builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\PROJEKTAI\\Data_storage_project_solution\\Data_storage_project\\Databases\\local_data_base.mdf;Integrated Security=True;Connect Timeout=30"));


builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ILoggerService, LoggerService>();

// Repository Registrations
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ICustomerContactRepository, CustomerContactRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IServiceRepository, ServiceRepository>();
builder.Services.AddScoped<ICurrencyRepository, CurrencyRepository>();
builder.Services.AddScoped<IStatusRepository, StatusRepository>();

// Service Registrations
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<ICustomerContactService, CustomerContactService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IServiceService, ServiceService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<ICurrencyService, CurrencyService>();
builder.Services.AddScoped<IStatusService, StatusService>();

// Project-Specific Registrations
builder.Services.AddScoped<IProjectIdGenerator, ProjectIdGenerator>();



var app = builder.Build();


app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

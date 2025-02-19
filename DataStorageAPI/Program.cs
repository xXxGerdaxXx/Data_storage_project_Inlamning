using Data_storage_project_library.Repositories;
using Data_storage_project_library.Contexts;
using Data_storage_project_library.Interfaces;
using Data_storage_project_library.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ? Add Controllers
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen(); // Enables Swagger UI for API testing

// ? Register DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\PROJEKTAI\\Data_storage_project_solution\\Data_storage_project\\Databases\\local_data_base.mdf;Integrated Security=True;Connect Timeout=30"));

// ? Register Generic Repository
builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

builder.Services.AddScoped<CustomerRepository>();
builder.Services.AddScoped<EmployeeRepository>();
builder.Services.AddScoped<ProjectRepository>();
builder.Services.AddScoped<RoleRepository>();

builder.Services.AddScoped<CustomerService>();
builder.Services.AddScoped<StatusService>();
builder.Services.AddScoped<ServiceService>();
builder.Services.AddScoped<CurrencyService>();
builder.Services.AddScoped<RoleService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IServiceService, ServiceService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<ICurrencyService, CurrencyService>();
builder.Services.AddScoped<IStatusService, StatusService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();

var app = builder.Build();

//// Enable Swagger UI for API testing
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

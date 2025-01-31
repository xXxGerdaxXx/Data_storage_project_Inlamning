using Data_storage_project_library.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data_storage_project_library.Contexts;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    // Registering all entities in the database
    public DbSet<CustomerEntity> Customers { get; set; }
    public DbSet<CustomerContactEntity> CustomerContacts { get; set; }
    public DbSet<ProjectEntity> Projects { get; set; }
    public DbSet<EmployeeEntity> Employees { get; set; }
    public DbSet<RoleEntity> Roles { get; set; }
    public DbSet<StatusTypeEntity> Statuses { get; set; }
    public DbSet<ServiceEntity> Services { get; set; }
    public DbSet<CurrencyEntity> Currencies { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configures customer & customerContact relationship
        modelBuilder.Entity<CustomerEntity>()
            .HasMany(c => c.CustomerContacts)
            .WithOne(cc => cc.Customer)
            .HasForeignKey(cc => cc.CustomerId)
            .OnDelete(DeleteBehavior.Cascade); // Deletes contacts when customer is deleted

        // Configures project relationships
        modelBuilder.Entity<ProjectEntity>()
            .HasOne(p => p.Customer)
            .WithMany()
            .HasForeignKey(p => p.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ProjectEntity>()
            .HasOne(p => p.Employee)
            .WithMany()
            .HasForeignKey(p => p.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ProjectEntity>()
            .HasOne(p => p.Status)
            .WithMany()
            .HasForeignKey(p => p.StatusId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ProjectEntity>()
            .HasOne(p => p.Service)
            .WithMany()
            .HasForeignKey(p => p.ServiceId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configures employee & role relationship
        modelBuilder.Entity<EmployeeEntity>()
            .HasOne(e => e.Role)
            .WithMany()
            .HasForeignKey(e => e.RoleId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

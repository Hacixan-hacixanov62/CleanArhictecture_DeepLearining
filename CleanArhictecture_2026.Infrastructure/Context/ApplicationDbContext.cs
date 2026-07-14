using CleanArhictecture_2026.Domain.Employee;
using Microsoft.EntityFrameworkCore;

namespace CleanArhictecture_2026.Infrastructure.Context;

internal sealed class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Employee> Employees { get; set; }
}

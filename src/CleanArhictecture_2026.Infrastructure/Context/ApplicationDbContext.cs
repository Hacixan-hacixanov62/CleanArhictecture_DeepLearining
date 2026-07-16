using CleanArhictecture_2026.Domain.Abstractions;
using CleanArhictecture_2026.Domain.Employee;
using CleanArhictecture_2026.Domain.Users;
using GenericRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CleanArhictecture_2026.Infrastructure.Context;

public sealed class ApplicationDbContext : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>, IUnitOfWork
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    public DbSet<Employee> Employees => Set<Employee>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        modelBuilder.Entity<Employee>().HasQueryFilter(x => !x.IsDeleted);

        modelBuilder.Ignore<IdentityUserClaim<Guid>>();
        modelBuilder.Ignore<IdentityRoleClaim<Guid>>();
        modelBuilder.Ignore<IdentityUserToken<Guid>>();   //Bunlar migration edende Identityde gelen bu classlari miqrasiya etmir .
        modelBuilder.Ignore<IdentityUserLogin<Guid>>();
        modelBuilder.Ignore<IdentityUserRole<Guid>>();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries<Entity>();

        HttpContextAccessor httpContextAccessor = new();
        string userIdString =
            httpContextAccessor
            .HttpContext!
            .User
            .Claims
            .First(p => p.Type == ClaimTypes.NameIdentifier)
            .Value;

        Guid userId = Guid.Parse(userIdString);

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Property(p => p.CreateAt)
                    .CurrentValue = DateTime.Now;
                entry.Property(p => p.CreateUserId)
                    .CurrentValue = userId;
            }

            if (entry.State == EntityState.Modified)
            {
                if(entry.Property(p => p.IsDeleted).CurrentValue ==true)
                {
                    entry.Property(p => p.DeleteAt)
                        .CurrentValue = DateTime.Now;
                    entry.Property(p => p.DeleteUserId)
                       .CurrentValue = userId;
                }
                else
                {
                    entry.Property(p => p.UpdateAt)
                        .CurrentValue = DateTime.Now;
                    entry.Property(p => p.UpdateUserId)
                       .CurrentValue = userId;
                }
            }

            if (entry.State == EntityState.Deleted)
            {
                throw new ArgumentException("Db'den direkt silme işlemi yapamazsınız");
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}

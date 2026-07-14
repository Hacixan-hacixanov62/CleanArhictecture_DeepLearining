using CleanArhictecture_2026.Domain.Employee;
using CleanArhictecture_2026.Infrastructure.Context;
using CleanArhictecture_2026.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace CleanArhictecture_2026.Infrastructure;

public static class InfrastructureRegistrar
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(opt =>
        {
            string connectionString = configuration.GetConnectionString("SqlServer")!;
            opt.UseSqlServer(connectionString);
        });

        services.Scan(opt => opt
        .FromAssemblies(typeof(InfrastructureRegistrar).Assembly)
        .AddClasses(publicOnly: false)
        .UsingRegistrationStrategy(RegistrationStrategy.Skip)
        .AsImplementedInterfaces()
        .WithScopedLifetime()
        );


        return services;
    }
}

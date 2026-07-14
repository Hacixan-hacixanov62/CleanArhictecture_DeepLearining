using CleanArhictecture_2026.Domain.Employee;
using CleanArhictecture_2026.Infrastructure.Context;
using GenericRepository;

namespace CleanArhictecture_2026.Infrastructure.Repositories;

internal sealed class EmployeeRepository : Repository<Employee, ApplicationDbContext>, IEmployeeRepository
{
    public EmployeeRepository(ApplicationDbContext context) : base(context)
    {
    }
}

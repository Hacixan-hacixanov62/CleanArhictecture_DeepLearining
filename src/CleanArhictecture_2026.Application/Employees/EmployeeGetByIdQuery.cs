using CleanArhictecture_2026.Domain.Employee;
using MediatR;
using TS.Result;


namespace CleanArhictecture_2026.Application.Employees;

public sealed record EmployeeGetByIdQuery(Guid Id) : IRequest<Result<Guid>>;

internal sealed class EmployeeGetByIdQueryHandler(
    IEmployeeRepository employeeRepository) : IRequestHandler<EmployeeGetByIdQuery, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(EmployeeGetByIdQuery request, CancellationToken cancellationToken)
    {
        var employee = await employeeRepository.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (employee is null)
        {
            return Result<Guid>.Failure("Employee not found.");
        }

        return Result<Guid>.Succeed(employee.Id);
    }
}
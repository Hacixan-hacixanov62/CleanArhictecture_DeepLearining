using MediatR;
using TS.Result;
using FluentValidation;
using GenericRepository;
using CleanArhictecture_2026.Domain.Employee;
using Mapster;


namespace CleanArhictecture_2026.Application.Employees;

public sealed record EmployeeDeleteCommand(Guid Id) : IRequest<Result<string>>;

internal sealed class EmployeeDeleteCommandValidator : AbstractValidator<EmployeeDeleteCommand>
{
    public EmployeeDeleteCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEqual(Guid.Empty).WithMessage("Geçerli bir Employee ID yazın");
    }
}

internal sealed class EmployeeDeleteCommandHandler(
    IEmployeeRepository employeeRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<EmployeeDeleteCommand, Result<string>>
{
    public async Task<Result<string>> Handle(EmployeeDeleteCommand request, CancellationToken cancellationToken)
    {
        var employee = await employeeRepository.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (employee is null)
        {
            return Result<string>.Failure("Employee not found.");
        }

        employee.IsDeleted= true;

        employeeRepository.Update(employee);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return "Employee deleted successfully.";
    }
}
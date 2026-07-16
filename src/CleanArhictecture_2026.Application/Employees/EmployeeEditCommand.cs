using CleanArhictecture_2026.Domain.Employee;
using FluentValidation;
using MediatR;
using TS.Result;
using GenericRepository;
using Mapster;

namespace CleanArhictecture_2026.Application.Employees;

public sealed record EmployeeEditCommand(
    Guid Id,
    string FirstName,
    string LastName,
    DateOnly BirthOfDate,
    decimal Salary,
    PersonalInformation PersonalInformation,
    Address? Address) : IRequest<Result<string>>;

public sealed class EmployeeEditCommandValidator : AbstractValidator<EmployeeEditCommand>
{
    public EmployeeEditCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEqual(Guid.Empty).WithMessage("Geçerli bir Employee ID yazın");
        RuleFor(x => x.FirstName).MinimumLength(3).WithMessage("Ad alanı en az 3 karakter olmalıdır");
        RuleFor(x => x.LastName).MinimumLength(3).WithMessage("Soyad alanı en az 3 karakter olmalıdır");
        RuleFor(x => x.PersonalInformation.TCNo)
            .MinimumLength(11).WithMessage("Geçerli bir TC Numarası yazın")
            .MaximumLength(11).WithMessage("Geçerli bir TC Numarası yazın");
    }
}

internal sealed class EmployeeEditCommandHandler(
    IEmployeeRepository employeeRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<EmployeeEditCommand, Result<string>>
{
    public async Task<Result<string>> Handle(EmployeeEditCommand request, CancellationToken cancellationToken)
    {
        var employee = await employeeRepository.FirstOrDefaultAsync(x =>x.Id == request.Id, cancellationToken);
        if (employee is null)
        {
            return Result<string>.Failure("Employee not found.");
        }

        request.Adapt(employee);

        employeeRepository.Update(employee);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return "Employee updated successfully.";
    }
}
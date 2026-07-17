using CleanArhictecture_2026.Domain.Employee;
using FluentValidation;
using GenericRepository;
using Mapster;
using MediatR;
using TS.Result;

namespace CleanArhictecture_2026.Application.Employees;
  //CQRS IN basqa formasidi cox istifade olunan.
public sealed record EmployeeCreateCommand(
    string FirstName,
    string LastName,
    DateOnly BirthOfDate,
    decimal Salary,
    PersonalInformation PersonalInformation,
    Address? Address,
    bool IsActive) : IRequest<Result<string>>;

public sealed class EmployeeCreateCommandValidator : AbstractValidator<EmployeeCreateCommand>
{
    public EmployeeCreateCommandValidator()
    {
        RuleFor(x => x.FirstName).MinimumLength(3).WithMessage("Ad alanı en az 3 karakter olmalıdır");
        RuleFor(x => x.LastName).MinimumLength(3).WithMessage("Soyad alanı en az 3 karakter olmalıdır");
        RuleFor(x => x.PersonalInformation.TCNo)
            .MinimumLength(11).WithMessage("Geçerli bir TC Numarası yazın")
            .MaximumLength(11).WithMessage("Geçerli bir TC Numarası yazın");
    }
}


internal sealed class EmployeeCreateCommandHandler(
    IEmployeeRepository employeeRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<EmployeeCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(EmployeeCreateCommand request, CancellationToken cancellationToken)
    {
        var isEmployeeExist = await employeeRepository.AnyAsync(x => x.PersonalInformation.TCNo == request.PersonalInformation.TCNo, cancellationToken);
        if (isEmployeeExist)
        {
            return Result<string>.Failure("Employee already exists.");
        }
        Employee employee = request.Adapt<Employee>(); // bu kod mapster nuget paketindeki adapt methodundan istifade olunub burada automaper kimi employee classindakilari commanda beraber edir .
                                                      // bir abalaca problem var eger property boyuk herfelerden cox istifade olunubsa ona diqqet etmek lazimdir .

        employeeRepository.Add(employee);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return "Employee created successfully.";
    }
}

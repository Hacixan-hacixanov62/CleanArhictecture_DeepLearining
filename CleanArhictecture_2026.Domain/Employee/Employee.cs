using CleanArhictecture_2026.Domain.Abstractions;

namespace CleanArhictecture_2026.Domain.Employee;

public sealed class Employee : Entity
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string FullName => string.Join(" ", FirstName, LastName);
    public DateOnly BirthOfDate { get; set; }
    public decimal Salary { get; set; }
    public Address? Address { get; set; }
    public PersonalInformation PersonalInformation { get; set; } =default!;

}

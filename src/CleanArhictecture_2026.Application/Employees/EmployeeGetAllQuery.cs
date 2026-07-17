using CleanArhictecture_2026.Domain.Abstractions;
using CleanArhictecture_2026.Domain.Employee;
using CleanArhictecture_2026.Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace CleanArhictecture_2026.Application.Employees;
public sealed record EmployeeGetAllQuery() : IRequest<IQueryable<EmployeeGetAllQueryResponse>>;
// Burada niye IQueryable den istifade olundu ?
// Cunki Database-de ki Employee-larin sayi cox ola biler ve biz butun Employee-lari bir anda memory-e yuklemek istemirik. IQueryable bize lazim olan Employee-lari lazim olan zaman memory-e yuklemeye imkan verir. Bu, performans baximindan daha effektivdir.
public sealed class EmployeeGetAllQueryResponse : EntityDto
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public DateOnly BirthOfDate { get; set; }
    public decimal Salary { get; set; }
    public string TCNo { get; set; } = default!;
}
internal sealed class EmployeeGetAllQueryHandler(
    IEmployeeRepository employeeRepository,
    UserManager<AppUser> userManager) : IRequestHandler<EmployeeGetAllQuery, IQueryable<EmployeeGetAllQueryResponse>>
{
    public Task<IQueryable<EmployeeGetAllQueryResponse>> Handle(EmployeeGetAllQuery request, CancellationToken cancellationToken)
    {
        //var response =employeeRepository.GetAll()
        //    .Join(userManager.Users.AsQueryable(), e => e.CreateUserId, u => u.Id, (create_user,entity) => new { create_user, entity});  // Join ile Employee-lar ile AppUser-lari birlesdiririk. Burada create_user Employee-dir, entity ise AppUser-dir. Bu join ile biz Employee-larin CreateUserId-si ile AppUser-larin Id-si eyni olanlari birlesdiririk.

        var response = (from employee in employeeRepository.GetAll()
                        join create_user in userManager.Users.AsQueryable() on employee.CreateUserId equals create_user.Id
                        join update_user in userManager.Users.AsQueryable() on employee.CreateUserId equals update_user.Id into update_user //inner join
                        from update_users in update_user.DefaultIfEmpty()
                        select new EmployeeGetAllQueryResponse
                        {
                            FirstName = employee.FirstName,
                            LastName = employee.LastName,
                            Salary = employee.Salary,
                            BirthOfDate = employee.BirthOfDate,
                            CreateAt = employee.CreateAt,
                            DeleteAt = employee.DeleteAt,
                            Id = employee.Id,
                            IsDeleted = employee.IsDeleted,
                            TCNo = employee.PersonalInformation.TCNo,
                            UpdateAt = employee.UpdateAt,
                            CreateUserId = employee.CreateUserId,
                            CreateUserName = create_user.FirstName + " " + create_user.LastName + " (" + create_user.Email + ")",
                            UpdateUserId = employee.UpdateUserId,
                            UpdateUserName = employee.UpdateUserId == null ? null : update_users.FirstName + " " + update_users.LastName + " (" + update_users.Email + ")",
                        });


        return Task.FromResult(response);
    }
}

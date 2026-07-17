using CleanArhictecture_2026.Domain.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArhictecture_2026.Domain.Abstractions;

public abstract class Entity
{
    public Entity()
    {
        Id = Guid.CreateVersion7();
    }
    public Guid Id { get; set; }
    #region Audit Log
    public bool IsActive { get; set; } = true;
    public DateTime CreateAt { get; set; } //DateTimeOffset
                                                 //niye DateTimeOffset kullandık ?
                                                 //Cunki basqa olkede olanda saat ferqine gore vaxt ferqi olacaq ve bu ferqi DateTimeOffset ile asanliqla hesablayacagik

    public Guid CreateUserId { get; set; } = default!;
    public string CreateUserName => GetCreateUserName();
    public DateTime? UpdateAt { get; set; }
    public Guid? UpdateUserId { get; set; }
    public string? UpdateUserName => GetUpdateUserName();
    public bool IsDeleted { get; set; }
    public DateTime? DeleteAt { get; set; }
    public Guid? DeleteUserId { get; set; }

    private string GetCreateUserName()
    {

        HttpContextAccessor httpContextAccessor = new();
        var userManager = httpContextAccessor
            .HttpContext
            .RequestServices
            .GetRequiredService<UserManager<AppUser>>();

        AppUser appUser = userManager.Users.First(p=>p.Id == CreateUserId);

        return appUser.FirstName+ " " + appUser.LastName + "(" + appUser.Email + ")";
    }

    private string? GetUpdateUserName()
    {
        if (UpdateUserId is null) return null;

        HttpContextAccessor httpContextAccessor = new();
        var userManager = httpContextAccessor
            .HttpContext
            .RequestServices
            .GetRequiredService<UserManager<AppUser>>();

        AppUser appUser = userManager.Users.First(p=>p.Id == UpdateUserId);

        return appUser.FirstName+ " " + appUser.LastName + "(" + appUser.Email + ")";
    } 
    #endregion

}
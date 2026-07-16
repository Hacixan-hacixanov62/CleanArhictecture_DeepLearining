using Microsoft.AspNetCore.Identity;

namespace CleanArhictecture_2026.Domain.Users;

public sealed class AppUser:IdentityUser<Guid>
{
    public AppUser()
    {
        Id = Guid.CreateVersion7();
    }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string FullName => $"{FirstName} {LastName}"; //computed property

    #region Audit Log
    public DateTime CreateAt { get; set; }
    public Guid CreateUserId { get; set; } = default!;
    public DateTime? UpdateAt { get; set; }
    public Guid? UpdateUserId { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeleteAt { get; set; }
    public Guid? DeleteUserId { get; set; }
    #endregion
}
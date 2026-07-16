using Microsoft.AspNetCore.Identity;
using CleanArhictecture_2026.Domain.Users;

namespace CleanArhictecture_2026.WebAPI;

public static class ExtensionMiddleware
{
    public static void CreateFirstUser(WebApplication app)
    {
        using(var scoped = app.Services.CreateScope())
        {
            var userManager =scoped.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

            if (!userManager.Users.Any(p => p.UserName == "admin"))
            {
                AppUser user = new()
                {
                    UserName = "admin",
                    Email = "admin@gmail.com",
                    FirstName = "Hajikhan",
                    LastName = "Hajikhanov",
                    EmailConfirmed = true,
                    CreateAt = DateTime.UtcNow
                };

                user.CreateUserId = user.Id;

                userManager.CreateAsync(user, "1").Wait();
            }
        }
    }
}

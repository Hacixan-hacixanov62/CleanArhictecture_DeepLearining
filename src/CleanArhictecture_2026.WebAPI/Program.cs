using CleanArhictecture_2026.Application;
using CleanArhictecture_2026.Domain.Users;
using CleanArhictecture_2026.Infrastructure.Context;
using CleanArhictecture_2026.Infrastructure;
using CleanArhictecture_2026.WebAPI;
using CleanArhictecture_2026.WebAPI.Controllers;
using CleanArhictecture_2026.WebAPI.Modules;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.RateLimiting;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);


builder.Services.AddControllers().AddOData(opt =>
        opt
        .Select()
        .Filter()
        .Count()
        .Expand()
        .OrderBy()
        .SetMaxTop(null)
        //.EnableQueryFeatures()
        .AddRouteComponents("odata", AppODataController.GetEdmModel())
//
); 

builder.Services.AddIdentity<AppUser, IdentityRole<Guid>>(opt =>
{
    opt.Password.RequiredLength = 1;
    opt.Password.RequireNonAlphanumeric = false;
    opt.Password.RequireDigit = false;
    opt.Password.RequireLowercase = false;
    opt.Password.RequireUppercase = false;
    opt.Lockout.MaxFailedAccessAttempts = 5;
    opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    opt.SignIn.RequireConfirmedEmail = false;
})
 .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders()
            .AddSignInManager<SignInManager<AppUser>>();

builder.Services.AddAuthorization();

builder.Services.AddCors();
builder.Services.AddOpenApi();
builder.Services.AddRateLimiter(x =>
    x.AddFixedWindowLimiter("fixed", cfg =>
    {
        cfg.QueueLimit = 100;
        cfg.Window = TimeSpan.FromSeconds(1);
        cfg.PermitLimit = 100;
        cfg.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
    }));

builder.Services.AddExceptionHandler<ExceptionHandler>().AddProblemDetails();

//builder.Services.AddAuthentication().AddJwtBearer();
//builder.Services.AddAuthorization();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseCors(x => x
    .AllowAnyHeader()
    .AllowCredentials()
    .AllowAnyMethod()
    .SetIsOriginAllowed(origin => true));

app.RegisterRoutes();

app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapDefaultEndpoints();

app.MapControllers().RequireAuthorization();

app.UseExceptionHandler();

app.MapControllerRoute(
   name: "areas",
   pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");                         //RequireAuthorization()
                                                                               //Bu method butun Controllerde avtomatik olaraq "Bearer" tokenini istifade etmeye mecbur edir.
                                                                               //Yalnizca [AllowAnonymous] attribute olan controllerler istisnadi.

ExtensionMiddleware.CreateFirstUser(app);

app.Run();

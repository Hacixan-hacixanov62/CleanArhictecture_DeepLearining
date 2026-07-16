using CleanArhictecture_2026.Application.Auth;
using CleanArhictecture_2026.Application.Employees;
using MediatR;
using TS.Result;

namespace CleanArhictecture_2026.WebAPI.Modules;

public static class AuthModule
{
    public static void RegisterAuthRoutes(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/auth").WithTags("Auth");

        group.MapPost("login",
            async (ISender sender, LoginCommand request, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
           .Produces<Result<LoginCommandResponse>>();
    }
}

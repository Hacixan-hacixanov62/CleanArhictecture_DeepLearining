using CleanArhictecture_2026.Application.Employees;
using MediatR;
using TS.Result;

namespace CleanArhictecture_2026.WebAPI.Modules;

public static class EmployeeModule
{
    public static void RegisterEmployeeRoutes(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group =app.MapGroup("/employees").WithTags("Employees").RequireAuthorization();

        group.MapPost(string.Empty,
            async (ISender sender, EmployeeCreateCommand request, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
           .Produces<Result<string>>();
        
        group.MapPut(string.Empty,
            async (ISender sender, EmployeeEditCommand request, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
           .Produces<Result<string>>();

        group.MapDelete("/{id:guid}",
            async (ISender sender, Guid id, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(new EmployeeDeleteCommand(id), cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            }) .Produces<Result<string>>();

    }
}

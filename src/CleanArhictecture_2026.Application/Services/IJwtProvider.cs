using CleanArhictecture_2026.Domain.Users;

namespace CleanArhictecture_2026.Application.Services;

public interface IJwtProvider
{
    public Task<string> CreateTokenAsync(AppUser user, CancellationToken cancellationToken =default);
}

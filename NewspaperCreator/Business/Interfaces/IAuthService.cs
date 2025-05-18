using Shared.Helpers;
using Shared.Requests.Auth;

namespace Business.Interfaces;

public interface IAuthService
{
    Task<Result<bool>> RegisterUserAsync(RegisterUserRequest request, CancellationToken cancellationToken = default);
    Task<Result<string>> SignIn(SignInRequest request, CancellationToken cancellationToken = default);
}

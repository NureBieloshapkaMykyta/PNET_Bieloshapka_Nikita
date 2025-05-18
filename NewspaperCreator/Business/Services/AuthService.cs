using AutoMapper;
using Business.Extensions;
using Business.Interfaces;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Shared.Helpers;
using Shared.Requests.Auth;

namespace Business.Abstractions;

public class AuthService(NewspaperDbContext dbContext, ITokenService tokenService, IMapper mapper) : IAuthService
{
    public async Task<Result<bool>> RegisterUserAsync(RegisterUserRequest request, CancellationToken cancellationToken = default)
    {
        var initialUser = new User()
        {
            Id = default,
            Email = request.Email,
            Username = request.Username,
            PasswordHash = HashExtension.GetHash(request.Password),
        };

        await dbContext.Users.AddAsync(initialUser, cancellationToken);

        await dbContext.SaveChangesAsync();

        return new Result<bool>(true);
    }

    public async Task<Result<string>> SignIn(SignInRequest request, CancellationToken cancellationToken = default)
    {
        var user = await dbContext.Users.FirstOrDefaultAsync(x => x.Email == request.Email);
        if (user is null || HashExtension.GetHash(request.Password) != user.PasswordHash) 
        {
            return new Result<string>(false);
        }

        return new Result<string>(true, data: tokenService.GenerateToken(user));
    }
}

using System.ComponentModel.DataAnnotations;

namespace Shared.Requests.Auth;

public record RegisterUserRequest(
    string Username,
    [EmailAddress]string Email,
    [MinLength(6)]string Password);

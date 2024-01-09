using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using YourRest.Domain.Entities;
using YourRest.Domain.Repositories;

namespace YourRest.Producer.Infrastructure.Middleware;
public class UserSavingMiddleware
{
    private readonly RequestDelegate _next;

    public UserSavingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext httpContext, IUserRepository userRepository, IAccommodationRepository accommodationRepository)
    {
        var identity = httpContext.User.Identity as ClaimsIdentity;

        if(identity == null)
        {
            throw new Exception("User not found.");
        }

        var sub = identity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var firstName = identity.FindFirst(ClaimTypes.GivenName)?.Value;
        var lastName = identity.FindFirst(ClaimTypes.Surname)?.Value;
        var email = identity.FindFirst(ClaimTypes.Email)?.Value;
        
        if (!string.IsNullOrEmpty(sub))
        {
            var users = await userRepository.GetWithIncludeAsync(
                a => a.KeyCloakId == sub, 
                cancellationToken: default
            );

            var user = users.FirstOrDefault();

            if (user == null)
            {
                user = new User
                {
                    KeyCloakId = sub,
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    UserAccommodations = new List<UserAccommodation>()
                };
                await userRepository.AddAsync(user);
            }
        }

        await _next(httpContext);
    }
}

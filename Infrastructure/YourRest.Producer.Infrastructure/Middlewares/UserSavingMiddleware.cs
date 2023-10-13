using YourRest.Domain.Entities;
using YourRest.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace YourRest.Producer.Infrastructure.Middlewares;

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
        var sub = identity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var firstName = identity?.FindFirst(ClaimTypes.GivenName)?.Value;
        var lastName = identity?.FindFirst(ClaimTypes.Surname)?.Value;
        var email = identity?.FindFirst(ClaimTypes.Email)?.Value;

        // group related to accommodation is in the format: /accommodations/{accommodationId}
        var allClaims = identity?.Claims;
        var accommodationGroupPattern = "/accommodations/";
        int? accommodationId = null;

        if (allClaims != null)
        {
            foreach (var claim in allClaims)
            {
                if (claim.Value.StartsWith(accommodationGroupPattern)) {
                    var accommodationIdStr = claim.Value.Substring(accommodationGroupPattern.Length);
                    if (int.TryParse(accommodationIdStr, out int result))
                    {
                        accommodationId = result;
                    }
                }                        
            }            
        }        

        if (!string.IsNullOrEmpty(sub))
        {
            var users = await userRepository.FindAsync(a => a.KeyCloakId == sub);
            var user = users.FirstOrDefault();

            if (accommodationId == null) {
                throw new Exception("Accommodation not found");
            }

            var accommodations = await accommodationRepository.GetWithIncludeAsync(a => a.Id == accommodationId);
            var accommodation = accommodations.FirstOrDefault();

            if (accommodation == null)
            {
                throw new Exception($"Accommodation with id {accommodationId} not found");
            }

            if (user == null)
            {
                user = new User
                {
                    KeyCloakId = sub,
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    AccommodationId = accommodation.Id
                };
                await userRepository.AddAsync(user);
            }
        }

        await _next(httpContext);
    }
}

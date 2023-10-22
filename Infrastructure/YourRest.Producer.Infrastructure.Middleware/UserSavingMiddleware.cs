using YourRest.Domain.Entities;
using YourRest.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Npgsql;

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
        var sub = identity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var firstName = identity?.FindFirst(ClaimTypes.GivenName)?.Value;
        var lastName = identity?.FindFirst(ClaimTypes.Surname)?.Value;
        var email = identity?.FindFirst(ClaimTypes.Email)?.Value;

        // group related to accommodation is in the format: /accommodations/{accommodationId}
        var allClaims = identity?.Claims;
        var accommodationGroupPattern = "/accommodations/";
        
        if (!string.IsNullOrEmpty(sub))
        {
            var users = await userRepository.GetWithIncludeAsync(a => a.KeyCloakId == sub, a => a.UserAccommodations);
            var user = users.FirstOrDefault();

            List<int> accommodationIds = new List<int>();
            if (allClaims != null)
            {
                foreach (var claim in allClaims)
                {
                    if (claim.Value.StartsWith(accommodationGroupPattern)) {
                        var accommodationIdStr = claim.Value.Substring(accommodationGroupPattern.Length);
                        if (int.TryParse(accommodationIdStr, out int result))
                        {
                            accommodationIds.Add(result);
                        }
                    }                        
                }            
            }

            if (accommodationIds.Count == 0) 
            {
                throw new Exception("No accommodations found from token groups");
            }

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

            foreach (var accommodationId in accommodationIds)
            {
                var accommodations = await accommodationRepository.GetWithIncludeAsync(a => a.Id == accommodationId);
                var accommodation = accommodations.FirstOrDefault();

                if (accommodation != null)
                {
                    var existingUserAccommodation = user.UserAccommodations.FirstOrDefault(ua => ua.AccommodationId == accommodation.Id);
                    if(existingUserAccommodation == null)
                    {
                        user.UserAccommodations.Add(new UserAccommodation { User = user, Accommodation = accommodation });
                    }
                }
            }

            await userRepository.UpdateAsync(user);
        }

        await _next(httpContext);
    }
}

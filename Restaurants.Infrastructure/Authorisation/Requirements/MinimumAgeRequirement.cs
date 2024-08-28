using Microsoft.AspNetCore.Authorization;

namespace Restaurants.Infrastructure.Authorisation.Requirements;

public class MinimumAgeRequirement(int minimumAge) : IAuthorizationRequirement
{
    public int MinimumAge { get; set; } = minimumAge;
}
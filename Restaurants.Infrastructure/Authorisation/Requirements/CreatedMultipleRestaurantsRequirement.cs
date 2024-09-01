using Microsoft.AspNetCore.Authorization;

namespace Restaurants.Infrastructure.Authorisation.Requirements;

public class CreatedMultipleRestaurantsRequirement(int minimumRestaurantsCreated) : IAuthorizationRequirement
{
        public int MinimumAccountsCreated { get; set; } = minimumRestaurantsCreated;
}
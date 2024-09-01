using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Users;
using Restaurants.Domain.IRepository;

namespace Restaurants.Infrastructure.Authorisation.Requirements;

public class CreatedMultipleRestaurantsRequirementHandler(
    ILogger<CreatedMultipleRestaurantsRequirementHandler> logger,
    IRestaurantRepository restaurantRepository, 
    IUserContext userContext) : AuthorizationHandler<CreatedMultipleRestaurantsRequirement>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, CreatedMultipleRestaurantsRequirement requirement)
    {
        var currentUser = userContext.GetCurrentUser();
        var restaurants = await restaurantRepository.GetAllRestaurantsAsync();
        var userRestaurantsCreated = restaurants.Count(r => r.OwnerId == currentUser!.Id);

        if (userRestaurantsCreated >= requirement.MinimumAccountsCreated)
        {
            logger.LogInformation("User: {Email}, created {CreatedRestaurants} restaurants - Authorization succeeded", currentUser!.Email, userRestaurantsCreated);
            context.Succeed(requirement);
        }
        else
        {
            context.Fail();
        }
    }
}
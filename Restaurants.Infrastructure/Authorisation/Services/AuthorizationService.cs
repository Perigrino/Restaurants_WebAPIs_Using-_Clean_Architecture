using Microsoft.Extensions.Logging;
using Restaurants.Application.Users;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.IRepository;

namespace Restaurants.Infrastructure.Authorisation.Services;

public class AuthorizationService(ILogger<AuthorizationService> logger, IUserContext userContext) : IAuthorizationService
{
    public bool Authorise(Restaurant restaurant, ResourceOperation resourceOperation)
    {
        var user = userContext.GetCurrentUser();
        
        logger.LogInformation("Authorising a user {UserEmail}, to {Operation} for restaurant {RestaurantName}", user!.Email, resourceOperation, restaurant.Name);

        if (resourceOperation is ResourceOperation.Read or ResourceOperation.Create)
        {
            logger.LogInformation(("Create/read operation - successful authorisation"));
            return true;
        }
        
        if (resourceOperation == ResourceOperation.Delete && user.IsInRole(UserRoles.Administrator))
        {
            logger.LogInformation(("Administrator user, delete operation - successful authorisation"));
            return true;
        }
        
        if ((resourceOperation == ResourceOperation.Delete || resourceOperation == ResourceOperation.Update) && user.Id == restaurant.OwnerId)
        {
            logger.LogInformation(("Restaurant owner, update operation - successful authorisation"));
            return true;
        }
        
        return false;
    }
    
}
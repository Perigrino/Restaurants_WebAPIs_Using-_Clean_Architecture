using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;

namespace Restaurants.Domain.IRepository;

public interface  IAuthorizationService
{
    bool Authorise(Restaurant restaurant, ResourceOperation resourceOperation);
}
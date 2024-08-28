using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;

namespace Restaurants.Domain.IRepository;

public interface  IAuthorisationService
{
    bool Authorise(Restaurant restaurant, ResourceOperation resourceOperation);
}
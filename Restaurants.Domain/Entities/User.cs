using Microsoft.AspNetCore.Identity;
using Restaurants.Domain.Entities;

namespace Restaurants.Domain.Entities;

public class User : IdentityUser
{
    public DateOnly? DateOfBirth { get; set; }
    public string? Nationality { get; set; }

    public List<Restaurant> OwnedRestaurants { get; set; } = [];
}
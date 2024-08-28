using System.Runtime.InteropServices.JavaScript;
using Microsoft.AspNetCore.Identity;
using Restaurants.Domain.Entites;

namespace Restaurants.Domain.Entities;

public class User : IdentityUser
{
    public DateOnly? DateOfBirth { get; set; }
    public string? Nationality { get; set; }

    public List<Restaurant> OwnedRestaurants { get; set; } = [];
}